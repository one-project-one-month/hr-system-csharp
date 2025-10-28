CREATE DATABASE [HRSystem];
USE [HRSystem];

-- table name: Tbl_Role
CREATE TABLE Tbl_Role (
    RoleId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    RoleCode NVARCHAR(50) NOT NULL UNIQUE,
    RoleName NVARCHAR(200),
    UniqueName NVARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_MenuGroup
CREATE TABLE Tbl_MenuGroup (
    MenuGroupId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    MenuGroupCode NVARCHAR(50) NOT NULL UNIQUE,
    MenuGroupName NVARCHAR(200),
    Url NVARCHAR(200),
    Icon NVARCHAR(200),
    SortOrder INT,
    HasMenuItem BIT,
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_Menu
CREATE TABLE Tbl_Menu (
    MenuId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    MenuCode NVARCHAR(50) NOT NULL UNIQUE,
    MenuGroupCode NVARCHAR(50),
    MenuName NVARCHAR(200),
    Url NVARCHAR(200),
    Icon NVARCHAR(200),
    SortOrder INT,
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_RoleAndMenuPermission
CREATE TABLE Tbl_RoleAndMenuPermission (
    RoleAndMenuPermissionId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    RoleAndMenuPermissionCode NVARCHAR(50) UNIQUE,
    RoleCode NVARCHAR(50),
    MenuGroupCode NVARCHAR(50),
    MenuCode NVARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_Sequence
CREATE TABLE Tbl_Sequence (
    SequenceId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    UniqueName NVARCHAR(50),
    SequenceNo NVARCHAR(50),
    SequenceDate DATETIME,
    SequenceType NVARCHAR(50),
    RoleCode NVARCHAR(50),
    DeleteFlag BIT
);

-- table name: Tbl_Project
CREATE TABLE Tbl_Project (
    ProjectId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    ProjectCode NVARCHAR(50) NOT NULL UNIQUE,
    ProjectName NVARCHAR(200),
    ProjectDescription NVARCHAR(200),
    StartDate DATETIME,
    EndDate DATETIME,
    ProjectStatus NVARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_EmployeeProject
CREATE TABLE Tbl_EmployeeProject (
    EmployeeProjectId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    EmployeeProjectCode NVARCHAR(50) NOT NULL UNIQUE,
    ProjectCode NVARCHAR(50),
    EmployeeCode NVARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_Verification
CREATE TABLE Tbl_Verification (
    VerificationId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    VerificationCode NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(200),
    ExpiredTime DATETIME,
    IsUsed BIT,
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_CompanyRule
CREATE TABLE Tbl_CompanyRule (
    CompanyRuleId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    CompanyRuleCode NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(200),
    Value NVARCHAR(50),
    IsActive BIT,
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_Employee
CREATE TABLE Tbl_Employee (
    EmployeeId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    EmployeeCode NVARCHAR(50) NOT NULL UNIQUE,
    RoleCode NVARCHAR(50),
    Username Nvarchar(200),
    Name NVARCHAR(200),
    Email NVARCHAR(200),
    Password NVARCHAR(200),
    PhoneNo NVARCHAR(50),
    ProfileImage NVARCHAR(200),
    StartDate DATETIME,
    ResignDate DATETIME,
    Salary DECIMAL(18,2),
    IsFirstTimeLogin BIT,
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_Location
CREATE TABLE Tbl_Location (
    LocationId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    LocationCode NVARCHAR(50) NOT NULL UNIQUE,
    Name NVARCHAR(50),
    Latitude NVARCHAR(50),
    Longitude NVARCHAR(50),
    Radius NVARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(50),
    ModifiedBy NVARCHAR(50),
    ModifiedAt DATETIME,
    DeleteFlag BIT
);

-- table name: Tbl_Task
CREATE TABLE Tbl_Task (
    TaskId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    TaskCode NVARCHAR(50) NOT NULL UNIQUE,
    EmployeeCode NVARCHAR(50),
    ProjectCode NVARCHAR(50),
    TaskName NVARCHAR(50),
    TaskDescription NVARCHAR(200),
    StartDate DATETIME,
    EndDate DATETIME,
    TaskStatus NVARCHAR(50),
    WorkingHour DECIMAL(4,2),
    CreatedAt DATETIME,
    CreatedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    DeleteFlag BIT
);

-- table name: Tbl_Attendance
CREATE TABLE Tbl_Attendance (
    AttendanceId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    AttendanceCode NVARCHAR(50) NOT NULL UNIQUE,
    EmployeeCode NVARCHAR(50),
    AttendanceDate DATETIME,
    CheckInTime DATETIME,
    CheckInLocation NVARCHAR(50),
    CheckOutTime DATETIME,
    CheckOutLocation NVARCHAR(50),
    WorkingHour DECIMAL(4,2),
    HourLateFlag INT,
    HalfDayFlag INT,
    FullDayFlag INT,
    Remark NVARCHAR(200),
    IsSavedLocation BIT,
    CreatedBy NVARCHAR(200),
    CreatedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    DeleteFlag BIT
);

-- table name: Tbl_Payroll
CREATE TABLE Tbl_Payroll (
    PayrollId NVARCHAR(200) NOT NULL PRIMARY KEY, 
    PayrollCode NVARCHAR(50) NOT NULL UNIQUE,
    EmployeeCode NVARCHAR(50),
    PayrollDate DATETIME,
    Status NVARCHAR(50),
    TotalWorkingHour INT,
    LeaveHour DECIMAL(8,2),
    ActualWorkingHour DECIMAL(8,2),
    BaseSalary DECIMAL(18,2),
    Allowance DECIMAL(18,2),
    GrossPay DECIMAL(18,2),
    Deduction DECIMAL(18,2),
    Tax DECIMAL(18,2),
    Bonus DECIMAL(18,2),
    NetPay DECIMAL(18,2),
    CreatedBy NVARCHAR(200),
    CreatedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    DeleteFlag BIT
);

-- table name: Tbl_RefreshToken
CREATE TABLE Tbl_RefreshToken (
    Token NVARCHAR(200) NOT NULL PRIMARY KEY,
    JwtId NVARCHAR(450) NOT NULL,
    ExpiryDate DATETIME2 NOT NULL,
    RevokedAt DATETIME2 NULL,
    IsRevoked BIT NOT NULL DEFAULT 0,
    -- need to changed to employee code
    EmployeeCode NVARCHAR(50) NOT NULL,
    CreatedBy NVARCHAR(200),
    CreatedAt DATETIME,
    ModifiedBy NVARCHAR(200),
    ModifiedAt DATETIME,
    DeleteFlag BIT
);