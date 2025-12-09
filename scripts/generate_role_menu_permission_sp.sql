CREATE OR ALTER PROCEDURE InitializeFullAdminPermissions
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleCode VARCHAR(50) = 'RL001';

    ---------------------------------------------------------------------
    -- 1️⃣ Insert menu-level permissions (for groups that have menus)
    ---------------------------------------------------------------------
    INSERT INTO Tbl_RoleAndMenuPermission (
        RoleAndMenuPermissionId,
        RoleAndMenuPermissionCode,
        RoleCode,
        MenuGroupCode,
        MenuCode,
        CreatedAt,
        CreatedBy,
        DeleteFlag,
        PermissionCode
    )
    SELECT 
        NEWID(),
        CONCAT('ADMIN-', mg.MenuGroupCode, '-', m.MenuCode, '-', p.PermissionCode) AS RoleAndMenuPermissionCode,
        @RoleCode,
        mg.MenuGroupCode,
        m.MenuCode,
        GETDATE(),
        'SYSTEM',
        0,
        p.PermissionCode
    FROM Tbl_MenuGroup mg
    INNER JOIN Tbl_Menu m ON m.MenuGroupCode = mg.MenuGroupCode       -- Only groups that have menus
    CROSS JOIN Tbl_Permission p
    WHERE NOT EXISTS (
        SELECT 1 
        FROM Tbl_RoleAndMenuPermission x
        WHERE x.RoleCode = @RoleCode
          AND x.MenuGroupCode = mg.MenuGroupCode
          AND x.MenuCode = m.MenuCode
          AND x.PermissionCode = p.PermissionCode
    );

    ---------------------------------------------------------------------
    -- 2️⃣ Insert group-level permissions (for groups without menus)
    ---------------------------------------------------------------------
    INSERT INTO Tbl_RoleAndMenuPermission (
        RoleAndMenuPermissionId,
        RoleAndMenuPermissionCode,
        RoleCode,
        MenuGroupCode,
        MenuCode,
        CreatedAt,
        CreatedBy,
        DeleteFlag,
        PermissionCode
    )
    SELECT 
        NEWID(),
        CONCAT('ADMIN-', mg.MenuGroupCode, '-X-', p.PermissionCode) AS RoleAndMenuPermissionCode,
        @RoleCode,
        mg.MenuGroupCode,
        NULL AS MenuCode,       -- Group-level permission
        GETDATE(),
        'SYSTEM',
        0,
        p.PermissionCode
    FROM Tbl_MenuGroup mg
    CROSS JOIN Tbl_Permission p
    WHERE NOT EXISTS (
        SELECT 1 
        FROM Tbl_RoleAndMenuPermission x
        WHERE x.RoleCode = @RoleCode
          AND x.MenuGroupCode = mg.MenuGroupCode
          AND x.MenuCode IS NULL
          AND x.PermissionCode = p.PermissionCode
    )
    AND NOT EXISTS (
        SELECT 1
        FROM Tbl_Menu m
        WHERE m.MenuGroupCode = mg.MenuGroupCode
    );  -- Only insert if the group has NO menus

    PRINT 'All missing ADMIN permissions inserted.';
END;
GO

-- Execute the procedure
USE HRSystem;
EXEC InitializeFullAdminPermissions;

-- Check results
SELECT * 
FROM Tbl_RoleAndMenuPermission
WHERE RoleCode = 'RL001'
ORDER BY MenuGroupCode, MenuCode, PermissionCode;
