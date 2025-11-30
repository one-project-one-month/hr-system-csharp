-- Admin Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V0',  -- RoleId (ULID format)
    'RL001',                        -- RoleCode
    'Administrator',                -- RoleName
    GETDATE(),                      -- CreatedAt
    'SYSTEM',                       -- CreatedBy
    NULL,                           -- ModifiedAt
    NULL,                           -- ModifiedBy
    0                               -- DeleteFlag (false)
);

-- Manager Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V1',
    'RL002',
    'Manager',
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- HR Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V2',
    'RL003',
    'HR Specialist',
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- Employee Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V3',
    'RL004',
    'Employee',
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- Additional Roles (Optional)

-- Senior Manager Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V4',
    'RL005',
    'Senior Manager',
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- Team Lead Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V5',
    'RL006',
    'Team Lead',
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- HR Manager Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V6',
    'RL007',
    'HR Manager',
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- Finance Role
INSERT INTO Tbl_Role (
    RoleId, RoleCode, RoleName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9V7',
    'RL008',
    'Finance Officer',
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);