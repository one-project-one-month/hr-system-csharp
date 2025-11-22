Use HRSystem;

ALTER TABLE Tbl_RoleAndMenuPermission DROP CONSTRAINT UQ__Tbl_Role__AB0007C896ABC595;

ALTER TABLE Tbl_RoleAndMenuPermission
ADD CONSTRAINT UQ_RoleMenu_Group UNIQUE (RoleAndMenuPermissionCode, MenuGroupCode, MenuCode, PermissionCode);


CREATE PROCEDURE GetPayrollList
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        PayrollId,
        PayrollCode,
        EmployeeCode,
        PayrollDate,
        Status,
        TotalWorkingHour,
        LeaveHour,
        ActualWorkingHour,
        BaseSalary,
        Allowance,
        GrossPay,
        Deduction,
        Tax,
        Bonus,
        NetPay,
        CreatedBy,
        CreatedAt,
        ModifiedBy,
        ModifiedAt,
        DeleteFlag
    FROM TblPayroll
    WHERE DeleteFlag = 0;
END;
GO
