INSERT INTO Tbl_Menu (
    MenuId,
    MenuCode,
    MenuGroupCode,
    MenuName,
    Url,
    Icon,
    SortOrder,
    CreatedAt,
    CreatedBy,
    DeleteFlag
)
VALUES

(NEWID(), 'BACKLOG', 'BACKLOG', 'Backlog', '/backlog', 'file-text', 1, GETDATE(), 'system', 0),
(NEWID(), 'PROJECT', 'BACKLOG', 'Project', '/project', 'folder-tree', 2, GETDATE(), 'system', 0),

(NEWID(), 'LOCATION', 'ATTENDANCE', 'Location', '/location', 'map-pin', 1, GETDATE(), 'system', 0),
(NEWID(), 'ATTENDANCE', 'ATTENDANCE', 'Attendance', '/attendance', 'clock', 2, GETDATE(), 'system', 0),

(NEWID(), 'MENU_ITEM', 'MENU', 'Menu Item', '/menu_item', 'menu', 1, GETDATE(), 'system', 0),

(NEWID(), 'MENU_GROUP', 'MENU', 'Menu Group', '/menu', 'menugroup_icon', 1, GETDATE(), 'system', 0),