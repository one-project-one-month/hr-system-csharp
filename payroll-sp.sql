Use HRSystem;

------------------Role menu permission table fixed ---------------------------------
ALTER TABLE Tbl_RoleAndMenuPermission DROP CONSTRAINT UQ__Tbl_Role__AB0007C896ABC595;

ALTER TABLE Tbl_RoleAndMenuPermission
ADD CONSTRAINT UQ_RoleMenu_Group UNIQUE (RoleAndMenuPermissionCode, MenuGroupCode, MenuCode, PermissionCode);


--------------------- Holiday Table -----------------------------

Use HRSystem;
CREATE TABLE Tbl_Holiday (
    HolidayId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    HolidayDate Date NOT NULL,
    Description NVARCHAR(100),
    IsWorkingHoliday BIT DEFAULT 0,
);

INSERT INTO Tbl_Holiday (HolidayId, HolidayDate, Description, IsWorkingHoliday)
VALUES
    (NEWID(), '2025-11-04', 'Full Moon Day of Tazaungmone', 0),
    (NEWID(), '2025-11-14', 'National Day (Myanmar)', 0);

INSERT INTO Tbl_Holiday (HolidayId, HolidayDate, Description, IsWorkingHoliday)
VALUES
    (NEWID(), '2025-10-05', 'Full Moon Day of Thadingyut', 0),
    (NEWID(), '2025-10-06', 'Thadingyut Holiday', 0),
    (NEWID(), '2025-10-07', 'Thadingyut Holiday', 0),
    (NEWID(), '2025-10-20', 'Deepavali (Diwali)', 0);

----------------------------------- leave ---------------------------------
CREATE TABLE Tbl_Leave (
    LeaveId VARCHAR(50) PRIMARY KEY,
    EmployeeCode VARCHAR(50) NOT NULL,
    LeaveType VARCHAR(30), -- Annual, Sick, Casual, Unpaid
    Reason VARCHAR(255),
    StartDate DATE,
    EndDate DATE,
    TotalHours DECIMAL(10,2),
    IsPaid BIT,
    Status VARCHAR(20), -- Pending, Approved, Rejected
    ApprovedBy VARCHAR(50) NULL,
    ApprovedAt DATETIME NULL,
    CreatedAt DATETIME NOT NULL,
    CreatedBy VARCHAR(50),
    DeleteFlag BIT DEFAULT 0
);

--------------------- Payroll --------------------------------
USE HRSystem;
GO

CREATE PROCEDURE GetPayrollList
(   @Bonus DECIMAL(15,0) = 0,
    @EmployeeCode VARCHAR(50),
    @TaxRate DECIMAL(5,2) = 0.05   -- 5% default tax
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Today DATE = GETDATE();

    -- Payroll end date is always 20th of current month if today >= 20
    -- Otherwise, end date is 20th of previous month
    DECLARE @PayrollEnd DATE;
    DECLARE @PayrollStart DATE;

    IF DAY(@Today) >= 20
    BEGIN
    -- End date = 20th of current month
        SET @PayrollEnd = DATEFROMPARTS(YEAR(@Today), MONTH(@Today), 20);
    -- Start date = 20th of previous month
        SET @PayrollStart = DATEADD(MONTH, -1, @PayrollEnd);
    END
    ELSE
    BEGIN
    -- End date = 20th of previous month
        SET @PayrollEnd = DATEADD(MONTH, -1, DATEFROMPARTS(YEAR(@Today), MONTH(@Today), 20));
    -- Start date = 20th of two months ago
        SET @PayrollStart = DATEADD(MONTH, -1, @PayrollEnd);
    END

    SELECT @PayrollStart AS PayrollStartDate, @PayrollEnd AS PayrollEndDate;

    ------------------------ start of payroll -----------------
    DECLARE @PayrollCode VARCHAR(50) = CONCAT('PYR-', FORMAT(GETDATE(),'yyyyMMddHHmmss'));
    
    --------------------------------leave summary ---------------------------------
    ;WITH LeaveSummary AS (
    SELECT 
        EmployeeCode,
        SUM(CASE WHEN IsPaid = 0 THEN TotalHours ELSE 0 END) AS UnpaidLeaveHours,
        SUM(CASE WHEN IsPaid = 1 THEN TotalHours ELSE 0 END) AS PaidLeaveHours
    FROM Tbl_Leave
    WHERE EmployeeCode = @EmployeeCode
      AND Status = 'Approved'
      AND DeleteFlag = 0
      AND StartDate BETWEEN @PayrollStart AND @PayrollEnd
    GROUP BY EmployeeCode
    ),
    

    ---------------------------- Attendance Summary ------------------------------------

    AttSummary AS (
        SELECT 
            A.EmployeeCode,
            SUM(A.WorkingHour) AS TotalWorkingHour,
            E.Name,
            E.Salary AS BaseSalary
        FROM Tbl_Attendance A
        INNER JOIN Tbl_Employee E
            ON A.EmployeeCode = E.EmployeeCode
        LEFT JOIN Tbl_Holiday H
            ON A.AttendanceDate = H.HolidayDate
        WHERE A.EmployeeCode = @EmployeeCode
          AND AttendanceDate BETWEEN @PayrollStart AND @PayrollEnd
          AND A.DeleteFlag = 0
        GROUP BY A.EmployeeCode, E.Name, E.Salary
    )

    INSERT INTO Tbl_Payroll
    (
        PayrollId,
        PayrollCode,
        EmployeeCode,
        PayrollDate,
        TotalWorkingHour,
        LeaveHour,
        ActualWorkingHour,
        BaseSalary,
        Bonus,
        GrossPay,
        Deduction,
        Tax,
        NetPay,
        Status,
        CreatedBy,
        CreatedAt,
        DeleteFlag
    )
    SELECT
        NEWID(),
        @PayrollCode,
        @EmployeeCode,
        @PayrollEnd,
        TotalWorkingHour,
        L.UnpaidLeaveHours,
        (TotalWorkingHour - L.UnpaidLeaveHours) AS ActualWorkingHour,
        BaseSalary,
        @Bonus,
        (BaseSalary + @Bonus) AS GrossPay,
        (L.UnpaidLeaveHours * (BaseSalary / NULLIF(TotalWorkingHour, 0))) AS Deduction,
        ((BaseSalary + @Bonus) * @TaxRate) AS Tax,
        ((BaseSalary + @Bonus) - ((BaseSalary + @Bonus) * @TaxRate) 
            - (L.UnpaidLeaveHours * (BaseSalary / NULLIF(TotalWorkingHour, 0)))) AS NetPay,
        'FINALIZED',
        'System',
        GETDATE(),
        0
    FROM AttSummary A
    LEFT JOIN LeaveSummary L
    ON A.EmployeeCode = L.EmployeeCode;

    SELECT PayrollId, PayrollCode, EmployeeCode, PayrollDate, TotalWorkingHour, LeaveHour,
       ActualWorkingHour, BaseSalary, Bonus, GrossPay, Deduction, Tax, NetPay,
       Status, CreatedBy, CreatedAt, DeleteFlag
    FROM Tbl_Payroll
    WHERE PayrollCode = @PayrollCode

END;
GO


DROP PROCEDURE GetPayrollList