
CREATE OR ALTER PROCEDURE InitializeFullAdminPermissions
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleCode VARCHAR(50) = 'RL001';

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
        CONCAT('ADMIN-', mg.MenuGroupCode, '-', ISNULL(m.MenuCode, 'X'), '-', p.PermissionCode) AS Id,
        @RoleCode,
        mg.MenuGroupCode,
        m.MenuCode,
        GETDATE(),
        'SYSTEM',
        0,
        p.PermissionCode
    FROM Tbl_MenuGroup mg
    LEFT JOIN Tbl_Menu m ON m.MenuGroupCode = mg.MenuGroupCode
    CROSS JOIN Tbl_Permission p
    WHERE NOT EXISTS (
        SELECT 1 
        FROM Tbl_RoleAndMenuPermission x
        WHERE x.RoleCode = @RoleCode
        AND x.MenuGroupCode = mg.MenuGroupCode
        AND (
            (x.MenuCode = m.MenuCode)
            OR (x.MenuCode IS NULL AND m.MenuCode IS NULL)
        )
        AND x.PermissionCode = p.PermissionCode
    );

    PRINT 'All missing ADMIN permissions inserted.';
END;
GO

USE HRSystem;
exec InitializeFullAdminPermissions;

select * from Tbl_RoleAndMenuPermission;