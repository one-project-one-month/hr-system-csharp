CREATE OR ALTER PROCEDURE InitializeFullAdminPermissions
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleCode VARCHAR(50) = 'RL001';

    ------------------------------------------------------
    -- 1. Ensure ADMIN role exists
    ------------------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM Tbl_Role WHERE RoleCode = @RoleCode)
    BEGIN
        INSERT INTO Tbl_Role
        (RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, DeleteFlag)
        VALUES
        (NEWID(), @RoleCode, 'Administrator', GETDATE(), 'SYSTEM', 0);
    END

    ------------------------------------------------------
    -- 2. DASHBOARD & PAYROLL (no menu, no permission)
    ------------------------------------------------------
    INSERT INTO Tbl_RoleAndMenuPermission
    (
        RoleAndMenuPermissionId,
        RoleAndMenuPermissionCode,
        RoleCode,
        MenuGroupCode,
        MenuCode,
        PermissionCode,
        CreatedAt,
        CreatedBy,
        DeleteFlag
    )
    SELECT
        NEWID(),
        CONCAT('ADMIN-', mg.MenuGroupCode),
        @RoleCode,
        mg.MenuGroupCode,
        NULL,
        NULL,
        GETDATE(),
        'SYSTEM',
        0
    FROM Tbl_MenuGroup mg
    WHERE mg.MenuGroupCode IN ('DASHBOARD', 'PAYROLL', 'ROLE_MENU_PERMISSION')
    AND NOT EXISTS (
        SELECT 1 FROM Tbl_RoleAndMenuPermission x
        WHERE x.RoleCode = @RoleCode
        AND x.MenuGroupCode = mg.MenuGroupCode
    );

    ------------------------------------------------------
    -- 3. COMPANY RULES (permissions, no menu items)
    ------------------------------------------------------
    INSERT INTO Tbl_RoleAndMenuPermission
    (
        RoleAndMenuPermissionId,
        RoleAndMenuPermissionCode,
        RoleCode,
        MenuGroupCode,
        MenuCode,
        PermissionCode,
        CreatedAt,
        CreatedBy,
        DeleteFlag
    )
    SELECT
        NEWID(),
        CONCAT('ADMIN-COMPANY_RULES-', p.PermissionCode),
        @RoleCode,
        'COMPANY_RULES',
        NULL,
        p.PermissionCode,
        GETDATE(),
        'SYSTEM',
        0
    FROM Tbl_Permission p
    WHERE p.PermissionCode IN ('LIST','UPDATE')
    AND NOT EXISTS (
        SELECT 1 FROM Tbl_RoleAndMenuPermission x
        WHERE x.RoleCode = @RoleCode
        AND x.MenuGroupCode = 'COMPANY_RULES'
        AND x.PermissionCode = p.PermissionCode
    );

    ------------------------------------------------------
    -- 4. NORMAL MODULES (menu + full permissions)
    ------------------------------------------------------
    INSERT INTO Tbl_RoleAndMenuPermission
    (
        RoleAndMenuPermissionId,
        RoleAndMenuPermissionCode,
        RoleCode,
        MenuGroupCode,
        MenuCode,
        PermissionCode,
        CreatedAt,
        CreatedBy,
        DeleteFlag
    )
    SELECT
        NEWID(),
        CONCAT(
            'ADMIN-',
            mg.MenuGroupCode, '-',
            m.MenuCode, '-',
            p.PermissionCode
        ),
        @RoleCode,
        mg.MenuGroupCode,
        m.MenuCode,
        p.PermissionCode,
        GETDATE(),
        'SYSTEM',
        0
    FROM Tbl_MenuGroup mg
    JOIN Tbl_Menu m
        ON m.MenuGroupCode = mg.MenuGroupCode
    CROSS JOIN Tbl_Permission p
    WHERE mg.MenuGroupCode NOT IN ('DASHBOARD', 'PAYROLL', 'COMPANY_RULES')
    AND NOT EXISTS (
        SELECT 1 FROM Tbl_RoleAndMenuPermission x
        WHERE x.RoleCode = @RoleCode
        AND x.MenuGroupCode = mg.MenuGroupCode
        AND x.MenuCode = m.MenuCode
        AND x.PermissionCode = p.PermissionCode
    );

    PRINT 'ADMIN permissions initialized with inline rules.';
END;
GO

EXEC InitializeFullAdminPermissions;

SELECT * FROM Tbl_RoleAndMenuPermission;
