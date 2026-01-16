INSERT INTO Tbl_MenuGroup (
    MenuGroupId,
    MenuGroupCode,
    MenuGroupName,
    HasMenuItem,
    Url,
    Icon,
    SortOrder,
    CreatedAt,
    CreatedBy,
    ModifiedAt,
    ModifiedBy,
    DeleteFlag
)
VALUES
-- 1. Role
(NEWID(), 'ROLE', 'Role', 0, '/role', 'fa-user-shield', 2, GETDATE(), 'system', NULL, NULL, 0),

-- 2. Dashboard
(NEWID(), 'DASHBOARD', 'Dashboard', 0, '/dashboard', 'fa-chart-line', 1, GETDATE(), 'system', NULL, NULL, 0),

-- 3. Employee
(NEWID(), 'EMPLOYEE', 'Employee', 0, '/employee', 'fa-users', 3, GETDATE(), 'system', NULL, NULL, 0),

-- 4. Menu
(NEWID(), 'MENU', 'Menu', 1, '/menu', 'fa-list', 4, GETDATE(), 'system', NULL, NULL, 0),

-- 5. Backlog Management
(NEWID(), 'BACKLOG', 'Backlog Management', 1, '/backlog', 'fa-tasks', 5, GETDATE(), 'system', NULL, NULL, 0),

-- 6. Attendance Management
(NEWID(), 'ATTENDANCE', 'Attendance Management', 1, '/attendance', 'fa-calendar-check', 6, GETDATE(), 'system', NULL, NULL, 0),

-- 7. Payroll
(NEWID(), 'PAYROLL', 'Payroll', 0, '/payroll', 'fa-money-check-alt', 7, GETDATE(), 'system', NULL, NULL, 0),

(NEWID(), 'ROLE_MENU_PERMISSION', 'Role and Menu Permission', 0, '/role-and-menu-permissions', 'fa-money-check-alt', 8, GETDATE(), 'system', NULL, NULL, 0),
(NEWID(), 'COMPANY_RULES', 'Company Rules', 0, '/company-rules', 'fa-money-check-alt', 9, GETDATE(), 'system', NULL, NULL, 0),
(NEWID(), 'LEAVE', 'Leave', 0, '/leave', 'fa-money-check-alt', 9, GETDATE(), 'system', NULL, NULL, 0);

