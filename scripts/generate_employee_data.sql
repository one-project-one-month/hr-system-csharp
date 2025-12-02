-- Admin User
INSERT INTO Tbl_Employee (
    EmployeeId, EmployeeCode, RoleCode, Username, Name, Email, Password, 
    PhoneNo, ProfileImage, StartDate, ResignDate, Salary, IsFirstTimeLogin, 
    CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9U0',  -- EmployeeId (ULID format)
    'EMP001',                       -- EmployeeCode
    'RL001',                        -- RoleCode (Admin role - adjust as needed)
    'admin',                        -- Username
    'System Administrator',         -- Name
    'admin@hrsystem.com',           -- Email
    '$2a$12$WYV58dPKP2HxpyPnGch3UO3USV8cOF1Is3dwchkgLWv0bL6Y8uu/S',  -- Password: Admin123! (BCrypt hash - REPLACE with actual hash)
    '+1234567890',                  -- PhoneNo
    '',                             -- ProfileImage
    '2024-01-01',                   -- StartDate
    NULL,                           -- ResignDate
    100000.00,                      -- Salary
    0,                              -- IsFirstTimeLogin (false)
    GETDATE(),                      -- CreatedAt
    'SYSTEM',                       -- CreatedBy
    NULL,                           -- ModifiedAt
    NULL,                           -- ModifiedBy
    0                               -- DeleteFlag (false)
);

-- Manager User
INSERT INTO Tbl_Employee (
    EmployeeId, EmployeeCode, RoleCode, Username, Name, Email, Password, 
    PhoneNo, ProfileImage, StartDate, ResignDate, Salary, IsFirstTimeLogin, 
    CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9U1',
    'EMP002',
    'RL002',                        -- RoleCode (Manager role - adjust as needed)
    'manager',
    'John Manager',
    'manager@hrsystem.com',
    '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyY5Y5Y5Y5Y5',  -- Password: Manager123! (BCrypt hash - REPLACE with actual hash)
    '+1234567891',
    '',
    '2024-01-15',
    NULL,
    75000.00,
    0,
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- HR User
INSERT INTO Tbl_Employee (
    EmployeeId, EmployeeCode, RoleCode, Username, Name, Email, Password, 
    PhoneNo, ProfileImage, StartDate, ResignDate, Salary, IsFirstTimeLogin, 
    CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9U2',
    'EMP003',
    'RL003',                        -- RoleCode (HR role - adjust as needed)
    'hr',
    'Jane HR Specialist',
    'hr@hrsystem.com',
    '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyY5Y5Y5Y5Y5',  -- Password: HR123! (BCrypt hash - REPLACE with actual hash)
    '+1234567892',
    '',
    '2024-02-01',
    NULL,
    65000.00,
    0,
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- Employee 1
INSERT INTO Tbl_Employee (
    EmployeeId, EmployeeCode, RoleCode, Username, Name, Email, Password, 
    PhoneNo, ProfileImage, StartDate, ResignDate, Salary, IsFirstTimeLogin, 
    CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9U3',
    'EMP004',
    'RL004',                        -- RoleCode (Employee role - adjust as needed)
    'employee1',
    'Alice Employee',
    'alice@hrsystem.com',
    '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyY5Y5Y5Y5Y5',  -- Password: Password123! (BCrypt hash - REPLACE with actual hash)
    '+1234567893',
    '',
    '2024-03-01',
    NULL,
    50000.00,
    1,                              -- IsFirstTimeLogin (true - needs to change password)
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- Employee 2
INSERT INTO Tbl_Employee (
    EmployeeId, EmployeeCode, RoleCode, Username, Name, Email, Password, 
    PhoneNo, ProfileImage, StartDate, ResignDate, Salary, IsFirstTimeLogin, 
    CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9U4',
    'EMP005',
    'RL004',                        -- RoleCode (Employee role - adjust as needed)
    'employee2',
    'Bob Employee',
    'bob@hrsystem.com',
    '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyY5Y5Y5Y5Y5',  -- Password: Password123! (BCrypt hash - REPLACE with actual hash)
    '+1234567894',
    '',
    '2024-03-15',
    NULL,
    52000.00,
    1,                              -- IsFirstTimeLogin (true - needs to change password)
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);

-- Employee 3
INSERT INTO Tbl_Employee (
    EmployeeId, EmployeeCode, RoleCode, Username, Name, Email, Password, 
    PhoneNo, ProfileImage, StartDate, ResignDate, Salary, IsFirstTimeLogin, 
    CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, DeleteFlag
) VALUES (
    '01JQZ8X9K2M3N4P5Q6R7S8T9U5',
    'EMP006',
    'RL004',                        -- RoleCode (Employee role - adjust as needed)
    'employee3',
    'Charlie Employee',
    'charlie@hrsystem.com',
    '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyY5Y5Y5Y5Y5',  -- Password: Password123! (BCrypt hash - REPLACE with actual hash)
    '+1234567895',
    '',
    '2024-04-01',
    NULL,
    48000.00,
    1,                              -- IsFirstTimeLogin (true - needs to change password)
    GETDATE(),
    'SYSTEM',
    NULL,
    NULL,
    0
);