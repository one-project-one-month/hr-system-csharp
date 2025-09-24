CREATE DATABASE [HRSystem];
USE [HRSystem];

-- Table: Tbl_Role
CREATE TABLE Tbl_Role (
    RoleId VARCHAR(200) NOT NULL PRIMARY KEY,
    RoleCode VARCHAR(50) NOT NULL UNIQUE,
    RoleName VARCHAR(200),
    UniqueName VARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_MenuGroup
CREATE TABLE Tbl_MenuGroup (
    MenuGroupId VARCHAR(200) NOT NULL PRIMARY KEY,
    MenuGroupCode VARCHAR(50) NOT NULL UNIQUE,
    MenuGroupName VARCHAR(200),
    Url VARCHAR(200),
    Icon VARCHAR(200),
    SortOrder INT,
    HasMenuGroup BIT,
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_Menu
CREATE TABLE Tbl_Menu (
    MenuId VARCHAR(200) NOT NULL PRIMARY KEY,
    MenuCode VARCHAR(50) NOT NULL UNIQUE,
    MenuGroupCode VARCHAR(50),
    MenuName VARCHAR(200),
    Url VARCHAR(200),
    Icon VARCHAR(200),
    SortOrder INT,
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_RoleAndMenuPermission
CREATE TABLE Tbl_RoleAndMenuPermission (
    RoleAndMenuPermissionId VARCHAR(200) NOT NULL PRIMARY KEY,
    RoleAndMenuPermissionCode VARCHAR(50),
    RoleCode VARCHAR(50),
    MenuGroupCode VARCHAR(50),
    MenuCode VARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_Sequence
CREATE TABLE Tbl_Sequence (
    SequenceId VARCHAR(200) NOT NULL PRIMARY KEY,
    UniqueName VARCHAR(50) NOT NULL,
    SequenceNo VARCHAR(50),
    SequenceDate DATETIME,
    SequenceType VARCHAR(50),
    RoleCode VARCHAR(50),
    DeleteFlag BIT
);

-- Table: Tbl_Project
CREATE TABLE Tbl_Project (
    ProjectId VARCHAR(200) NOT NULL PRIMARY KEY,
    ProjectCode VARCHAR(50) NOT NULL UNIQUE,
    ProjectName VARCHAR(200),
    ProjectDescription VARCHAR(200),
    StartDate DATETIME,
    EndDate DATETIME,
    ProjectStatus VARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_Employee
CREATE TABLE Tbl_Employee (
    EmployeeId VARCHAR(200) NOT NULL PRIMARY KEY,
    EmployeeCode VARCHAR(50) NOT NULL UNIQUE,
    RoleCode VARCHAR(50),
    Name VARCHAR(200),
    Email VARCHAR(200),
    Password VARCHAR(200),
	WrongPasswordCount INT,
	IsFirstTime BIT,
	IsLocked BIT,
    PhoneNo VARCHAR(50),
    ProfileImage VARCHAR(200),
    StartDate DATETIME,
    ResignDate DATETIME,
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_EmployeeProject
CREATE TABLE Tbl_EmployeeProject (
    EmployeeProjectId VARCHAR(200) NOT NULL PRIMARY KEY,
    EmployeeProjectCode VARCHAR(50) NOT NULL UNIQUE,
    ProjectCode VARCHAR(50),
    EmployeeCode VARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_Verification
CREATE TABLE Tbl_Verification (
    VerificationId VARCHAR(200) NOT NULL PRIMARY KEY,
    VerificationCode VARCHAR(50),
    Email VARCHAR(200),
    ExpiredTime DATETIME,
    IsUsed BIT,
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_CompanyRule
CREATE TABLE Tbl_CompanyRule (
    CompanyRuleId VARCHAR(200) NOT NULL PRIMARY KEY,
    CompanyRuleCode VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(200),
    Value VARCHAR(50),
    IsActive BIT,
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_Location
CREATE TABLE Tbl_Location (
    LocationId VARCHAR(200) NOT NULL PRIMARY KEY,
    LocationCode VARCHAR(50) NOT NULL UNIQUE,
    Name VARCHAR(50),
    Latitude VARCHAR(50),
    Longitude VARCHAR(50),
    Radius VARCHAR(50),
    CreatedAt DATETIME,
    CreatedBy VARCHAR(50),
    ModifiedBy VARCHAR(50),
    ModifiedAt DATETIME,
    DeleteFlag BIT
);

-- Table: Tbl_Task
CREATE TABLE Tbl_Task (
    TaskId VARCHAR(200) NOT NULL PRIMARY KEY,
    TaskCode VARCHAR(50) NOT NULL UNIQUE,
    EmployeeCode VARCHAR(50),
    ProjectCode VARCHAR(50),
    TaskName VARCHAR(50),
    TaskDescription VARCHAR(200),
    StartDate DATETIME,
    EndDate DATETIME,
    TaskStatus VARCHAR(50),
    WorkingHour DECIMAL(4, 2),
    CreatedAt DATETIME,
    CreatedBy VARCHAR(200),
    ModifiedAt DATETIME,
    ModifiedBy VARCHAR(200),
    DeleteFlag BIT
);

-- Table: Tbl_Attendance
CREATE TABLE Tbl_Attendance (
    AttendanceId VARCHAR(200) NOT NULL PRIMARY KEY,
    AttendanceCode VARCHAR(50) NOT NULL UNIQUE,
    AttendanceDate DATETIME,
    CheckInTime DATETIME,
    CheckInLocation VARCHAR(50),
    CheckOutTime DATETIME,
    CheckOutLocation VARCHAR(50),
    WorkingHour DECIMAL(4, 2),
    HourLateFlag INT,
    HalfDayFlag INT,
    FullDayFlag INT,
    Remark VARCHAR(200),
    IsSavedLocation BIT,
    CreatedBy VARCHAR(200),
    CreatedAt DATETIME,
    ModifiedBy VARCHAR(200),
    ModifiedAt DATETIME,
    DeleteFlag BIT
);

-- Table: Tbl_Payroll
CREATE TABLE Tbl_Payroll (
    PayrollId VARCHAR(200) NOT NULL PRIMARY KEY,
    PayrollCode VARCHAR(50) NOT NULL UNIQUE,
    EmployeeCode VARCHAR(50),
    PayrollDate DATETIME,
    PayrollStatus VARCHAR(50),
    BaseSalary DECIMAL(18, 2),
    Allowance DECIMAL(18, 2),
    TotalWorkingHour INT,
    LeaveHour DECIMAL(8, 2),
    ActualWorkingHour DECIMAL(8, 2),
    Deduction DECIMAL(18, 2),
    TotalPayroll DECIMAL(18, 2),
    Tax DECIMAL(18, 2),
    Bonus DECIMAL(18, 2),
    GrandTotalPayroll DECIMAL(18, 2)
);