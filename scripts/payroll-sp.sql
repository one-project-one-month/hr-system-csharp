Use HRSystem;

------------------Role menu permission table fixed ---------------------------------
ALTER TABLE Tbl_RoleAndMenuPermission DROP CONSTRAINT UQ__Tbl_Role__AB0007C8D0D3FC57;

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
    (NEWID(), '2025-10-20', 'Deepavali (Diwali)', 0)


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

END;
GO

INSERT INTO Tbl_Attendance
(
    AttendanceId,
    AttendanceCode,
    EmployeeCode,
    AttendanceDate,
    CheckInTime,
    CheckOutTime,
    WorkingHour,
    HourLateFlag,
    HalfDayFlag,
    FullDayFlag,
    DeleteFlag,
    CreatedAt
)
VALUES
-- October 2025
(NEWID(), 'ATD-20251020', 'EMP001', '2025-10-20', '2025-10-20 09:00:00', '2025-10-20 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251021', 'EMP001', '2025-10-21', '2025-10-21 09:00:00', '2025-10-21 13:00:00', 4, 0, 1, 0, 0, GETDATE()), -- Half day morning off
(NEWID(), 'ATD-20251022', 'EMP001', '2025-10-22', '2025-10-22 10:00:00', '2025-10-22 17:00:00', 7, 1, 0, 1, 0, GETDATE()), -- Late 1 hr
(NEWID(), 'ATD-20251023', 'EMP001', '2025-10-23', '2025-10-23 09:00:00', '2025-10-23 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251024', 'EMP001', '2025-10-24', '2025-10-24 09:00:00', '2025-10-24 13:00:00', 4, 0, 1, 0, 0, GETDATE()),

-- October 27-31
(NEWID(), 'ATD-20251027', 'EMP001', '2025-10-27', '2025-10-27 09:00:00', '2025-10-27 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251028', 'EMP001', '2025-10-28', '2025-10-28 09:30:00', '2025-10-28 17:00:00', 7.5, 0.5, 0, 1, 0, GETDATE()), -- Late 30 mins
(NEWID(), 'ATD-20251029', 'EMP001', '2025-10-29', '2025-10-29 09:00:00', '2025-10-29 13:00:00', 4, 0, 1, 0, 0, GETDATE()), -- Half day afternoon off
(NEWID(), 'ATD-20251030', 'EMP001', '2025-10-30', '2025-10-30 09:00:00', '2025-10-30 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251031', 'EMP001', '2025-10-31', '2025-10-31 09:15:00', '2025-10-31 17:00:00', 7.75, 0.25, 0, 1, 0, GETDATE()), -- Late 15 mins

-- November 2025
(NEWID(), 'ATD-20251107', 'EMP001', '2025-11-07', '2025-11-07 09:00:00', '2025-11-07 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251110', 'EMP001', '2025-11-10', '2025-11-10 09:00:00', '2025-11-10 13:00:00', 4, 0, 1, 0, 0, GETDATE()),

-- November 11-14
(NEWID(), 'ATD-20251111', 'EMP001', '2025-11-11', '2025-11-11 09:30:00', '2025-11-11 17:00:00', 7.5, 0.5, 0, 1, 0, GETDATE()), -- Late 30 mins
(NEWID(), 'ATD-20251112', 'EMP001', '2025-11-12', '2025-11-12 09:00:00', '2025-11-12 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251113', 'EMP001', '2025-11-13', '2025-11-13 09:00:00', '2025-11-13 13:00:00', 4, 0, 1, 0, 0, GETDATE()),
-- November 14 is a holiday, skipped

-- November 17-20
(NEWID(), 'ATD-20251117', 'EMP001', '2025-11-17', '2025-11-17 09:00:00', '2025-11-17 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251118', 'EMP001', '2025-11-18', '2025-11-18 09:00:00', '2025-11-18 17:00:00', 8, 0, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251119', 'EMP001', '2025-11-19', '2025-11-19 09:15:00', '2025-11-19 17:00:00', 7.75, 0.25, 0, 1, 0, GETDATE()),
(NEWID(), 'ATD-20251120', 'EMP001', '2025-11-20', '2025-11-20 09:00:00', '2025-11-20 17:00:00', 8, 0, 0, 1, 0, GETDATE());

INSERT INTO Tbl_Leave
(
    LeaveId,
    EmployeeCode,
    LeaveType,
    Reason,
    StartDate,
    EndDate,
    TotalHours,
    IsPaid,
    Status,
    ApprovedBy,
    ApprovedAt,
    CreatedAt,
    CreatedBy,
    DeleteFlag
)
VALUES
(
    NEWID(),
    'EMP001',
    'Annual',
    'Personal matters',
    '2025-11-05',
    '2025-11-06',
    16,
    1, -- Paid leave
    'Approved',
    'ADM-001',
    GETDATE(),
    GETDATE(),
    'ADM-001',
    0
);