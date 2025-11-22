USE [HRSystem]
GO
/****** Object:  Table [dbo].[Tbl_Attendance]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Attendance](
	[AttendanceId] [nvarchar](200) NOT NULL,
	[AttendanceCode] [nvarchar](50) NOT NULL,
	[EmployeeCode] [nvarchar](50) NULL,
	[AttendanceDate] [datetime2](7) NULL,
	[CheckInTime] [datetime2](7) NULL,
	[CheckInLocation] [nvarchar](50) NULL,
	[CheckOutTime] [datetime2](7) NULL,
	[CheckOutLocation] [nvarchar](50) NULL,
	[WorkingHour] [decimal](4, 2) NULL,
	[HourLateFlag] [int] NULL,
	[HalfDayFlag] [int] NULL,
	[FullDayFlag] [int] NULL,
	[Remark] [nvarchar](200) NULL,
	[IsSavedLocation] [bit] NULL,
	[CreatedBy] [nvarchar](200) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_CompanyRule]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_CompanyRule](
	[CompanyRuleId] [nvarchar](200) NOT NULL,
	[CompanyRuleCode] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Value] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Employee]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Employee](
	[EmployeeId] [nvarchar](200) NOT NULL,
	[EmployeeCode] [nvarchar](50) NOT NULL,
	[RoleCode] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[PhoneNo] [nvarchar](50) NOT NULL,
	[ProfileImage] [nvarchar](200) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[ResignDate] [datetime2](7) NULL,
	[Salary] [decimal](18, 2) NOT NULL,
	[IsFirstTimeLogin] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_EmployeeProject]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_EmployeeProject](
	[EmployeeProjectId] [nvarchar](200) NOT NULL,
	[EmployeeProjectCode] [nvarchar](50) NOT NULL,
	[ProjectCode] [nvarchar](50) NULL,
	[EmployeeCode] [nvarchar](50) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](200) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Location]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Location](
	[LocationId] [nvarchar](200) NOT NULL,
	[LocationCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Latitude] [nvarchar](50) NOT NULL,
	[Longitude] [nvarchar](50) NOT NULL,
	[Radius] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Menu]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Menu](
	[MenuId] [nvarchar](200) NOT NULL,
	[MenuCode] [nvarchar](50) NOT NULL,
	[MenuGroupCode] [nvarchar](50) NULL,
	[MenuName] [nvarchar](200) NOT NULL,
	[Url] [nvarchar](200) NULL,
	[Icon] [nvarchar](200) NULL,
	[SortOrder] [int] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MenuGroup]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MenuGroup](
	[MenuGroupId] [nvarchar](200) NOT NULL,
	[MenuGroupCode] [nvarchar](50) NOT NULL,
	[MenuGroupName] [nvarchar](200) NOT NULL,
	[Url] [nvarchar](200) NULL,
	[Icon] [nvarchar](200) NULL,
	[SortOrder] [int] NULL,
	[HasMenuItem] [bit] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MenuGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Payroll]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Payroll](
	[PayrollId] [nvarchar](200) NOT NULL,
	[PayrollCode] [nvarchar](50) NOT NULL,
	[EmployeeCode] [nvarchar](50) NULL,
	[PayrollDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[TotalWorkingHour] [int] NULL,
	[LeaveHour] [decimal](8, 2) NULL,
	[ActualWorkingHour] [decimal](8, 2) NULL,
	[BaseSalary] [decimal](18, 2) NULL,
	[Allowance] [decimal](18, 2) NULL,
	[GrossPay] [decimal](18, 2) NULL,
	[Deduction] [decimal](18, 2) NULL,
	[Tax] [decimal](18, 2) NULL,
	[Bonus] [decimal](18, 2) NULL,
	[NetPay] [decimal](18, 2) NULL,
	[CreatedBy] [nvarchar](200) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[DeleteFlag] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[PayrollId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Project]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Project](
	[ProjectId] [nvarchar](200) NOT NULL,
	[ProjectCode] [nvarchar](50) NOT NULL,
	[ProjectName] [nvarchar](200) NOT NULL,
	[ProjectDescription] [nvarchar](200) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[ProjectStatus] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_RefreshToken]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_RefreshToken](
	[Token] [nvarchar](200) NOT NULL,
	[JwtId] [nvarchar](450) NOT NULL,
	[ExpiryDate] [datetime2](7) NOT NULL,
	[RevokedAt] [datetime2](7) NULL,
	[IsRevoked] [bit] NOT NULL,
	[EmployeeCode] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](200) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[DeleteFlag] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Role]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Role](
	[RoleId] [nvarchar](200) NOT NULL,
	[RoleCode] [nvarchar](50) NOT NULL,
	[RoleName] [nvarchar](200) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_RoleAndMenuPermission]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_RoleAndMenuPermission](
	[RoleAndMenuPermissionId] [nvarchar](200) NOT NULL,
	[RoleAndMenuPermissionCode] [nvarchar](50) NULL,
	[RoleCode] [nvarchar](50) NOT NULL,
	[MenuGroupCode] [nvarchar](50) NULL,
	[MenuCode] [nvarchar](50) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleAndMenuPermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Sequence]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Sequence](
	[SequenceId] [nvarchar](200) NOT NULL,
	[UniqueName] [nvarchar](50) NULL,
	[SequenceNo] [nvarchar](50) NULL,
	[SequenceDate] [datetime] NULL,
	[SequenceType] [nvarchar](50) NULL,
	[DeleteFlag] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[SequenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Task]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Task](
	[TaskId] [nvarchar](200) NOT NULL,
	[TaskCode] [nvarchar](50) NOT NULL,
	[EmployeeCode] [nvarchar](50) NULL,
	[ProjectCode] [nvarchar](50) NULL,
	[TaskName] [nvarchar](50) NULL,
	[TaskDescription] [nvarchar](200) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[TaskStatus] [nvarchar](50) NULL,
	[WorkingHour] [decimal](4, 2) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](200) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Verification]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Verification](
	[VerificationId] [nvarchar](200) NOT NULL,
	[VerificationCode] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[ExpiredTime] [datetime] NULL,
	[IsUsed] [bit] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](200) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[DeleteFlag] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VerificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
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

(NEWID(), 'BACKLOG', 'BACKLOG', 'BACKLOG', '/backlog', 'file-text', 1, GETDATE(), 'system', 0),
(NEWID(), 'PROJECT', 'BACKLOG', 'BACKLOG', '/project', 'folder-tree', 2, GETDATE(), 'system', 0),

(NEWID(), 'LOCATION', 'ATTENDANCE', 'ATTENDANCE', '/location', 'map-pin', 1, GETDATE(), 'system', 0),
(NEWID(), 'ATTENDANCE', 'ATTENDANCE', 'ATTENDANCE', '/attendance', 'clock', 2, GETDATE(), 'system', 0),

(NEWID(), 'MENU_ITEM', 'MENU', 'MENU', '/menu-item', 'menu', 1, GETDATE(), 'system', 0),
(NEWID(), 'MENU_GROUP', 'MENU', 'MENU', '/menu-group', 'menugroup_icon', 1, GETDATE(), 'system', 0);
GO
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
(NEWID(), 'ROLE', 'Role', 0, '/role', 'fa-user-shield', 1, GETDATE(), 'system', NULL, NULL, 0),

-- 2. Dashboard
(NEWID(), 'DASHBOARD', 'Dashboard', 0, '/dashboard', 'fa-chart-line', 2, GETDATE(), 'system', NULL, NULL, 0),

(NEWID(), 'COMPANY_RULES', 'Company Rules', 0, '/company-rules', 'fa-chart-line', 2, GETDATE(), 'system', NULL, NULL, 0),
-- 3. Employee
(NEWID(), 'EMPLOYEE', 'Employee', 0, '/employee', 'fa-users', 3, GETDATE(), 'system', NULL, NULL, 0),

-- 4. Menu
(NEWID(), 'MENU', 'Menu', 1, '/menu', 'fa-list', 4, GETDATE(), 'system', NULL, NULL, 0),

-- 5. Backlog Management
(NEWID(), 'BACKLOG', 'Backlog Management', 1, '/backlog', 'fa-tasks', 5, GETDATE(), 'system', NULL, NULL, 0),

-- 6. Attendance Management
(NEWID(), 'ATTENDANCE', 'Attendance Management', 1, '/attendance', 'fa-calendar-check', 6, GETDATE(), 'system', NULL, NULL, 0),

-- 7. Payroll
(NEWID(), 'PAYROLL', 'Payroll', 0, '/payroll', 'fa-money-check-alt', 7, GETDATE(), 'system', NULL, NULL, 0);
GO
INSERT [dbo].[Tbl_Project] ([ProjectId], [ProjectCode], [ProjectName], [ProjectDescription], [StartDate], [EndDate], [ProjectStatus], [CreatedAt], [CreatedBy], [ModifiedAt], [ModifiedBy], [DeleteFlag]) VALUES (N'01K92EX13JP1PPMEZ7BH1VTZWF', N'PJ_251102_0001', N'ERP edit test', N'Deploy ERP system across departments', CAST(N'2025-10-21T00:00:00.0000000' AS DateTime2), CAST(N'2026-03-31T00:00:00.0000000' AS DateTime2), N'InProgress', CAST(N'2025-11-02T14:17:53.5266667' AS DateTime2), N'TestingUser', CAST(N'2025-11-02T14:20:09.2000000' AS DateTime2), N'TestingUser', 1)
INSERT [dbo].[Tbl_Project] ([ProjectId], [ProjectCode], [ProjectName], [ProjectDescription], [StartDate], [EndDate], [ProjectStatus], [CreatedAt], [CreatedBy], [ModifiedAt], [ModifiedBy], [DeleteFlag]) VALUES (N'01K92F4TMB3Y6SWBPXQCZQ1AVK', N'PJ_251102_0002', N'AML Project', N'', CAST(N'2025-11-01T00:00:00.0000000' AS DateTime2), CAST(N'2026-03-31T00:00:00.0000000' AS DateTime2), N'InProgress', CAST(N'2025-11-02T14:22:09.0366667' AS DateTime2), N'TestingUser', NULL, NULL, 0)
GO
INSERT [dbo].[Tbl_Role] ([RoleId], [RoleCode], [RoleName], [CreatedAt], [CreatedBy], [ModifiedAt], [ModifiedBy], [DeleteFlag]) VALUES (N'01K8D7W2R5TVQ981ZBH3BM6KTK', N'ADMIN', N'Admin', CAST(N'2025-10-25T15:00:59.3400000' AS DateTime2), N'admin', CAST(N'2025-10-25T15:03:53.3566667' AS DateTime2), N'admin', 0)
INSERT [dbo].[Tbl_Role] ([RoleId], [RoleCode], [RoleName], [CreatedAt], [CreatedBy], [ModifiedAt], [ModifiedBy], [DeleteFlag]) VALUES (N'01K900GJ9Y3YW482MHN37PWZF2', N'RL_251101_0001', N'HR', CAST(N'2025-11-01T15:27:56.2300000' AS DateTime2), N'admin', NULL, NULL, 0)
INSERT [dbo].[Tbl_Role] ([RoleId], [RoleCode], [RoleName], [CreatedAt], [CreatedBy], [ModifiedAt], [ModifiedBy], [DeleteFlag]) VALUES (N'R00001', N'EMPLOYEE', N'Employee', CAST(N'2025-10-25T15:02:19.6566667' AS DateTime2), N'admin', NULL, NULL, 0)
GO
CREATE TABLE [dbo].[Tbl_Permission](
[PermissionId] [nvarchar](200) NOT NULL,
[PermissionCode] [nvarchar](50) NOT NULL,
[PermissionName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
Go
ADD [PermissionCode] NVARCHAR(50) NOT NULL;
GO
GO
INSERT INTO Tbl_Permission(PermissionId, PermissionCode,PermissionName)
VALUES(NEWID(),'LIST', 'Listing');
INSERT INTO Tbl_Permission(PermissionId, PermissionCode,PermissionName)
VALUES(NEWID(),'DETAILS', 'Details');
INSERT INTO Tbl_Permission(PermissionId, PermissionCode,PermissionName)
VALUES(NEWID(),'CREATE', 'Create');
INSERT INTO Tbl_Permission(PermissionId, PermissionCode,PermissionName)
VALUES(NEWID(),'UPDATE', 'Update');
INSERT INTO Tbl_Permission(PermissionId, PermissionCode,PermissionName)
VALUES(NEWID(),'DELETE', 'Delete');
GO
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1c-7f6c-bf9a-09535b6a6d46', N'RL', N'0002', CAST(N'2025-11-01T22:02:27.697' AS DateTime), N'TABLE', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1d-7b04-bc11-8583b3210530', N'USR', N'0001', CAST(N'2025-10-25T18:57:45.840' AS DateTime), N'TABLE', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1e-7398-97ef-38d6a089783d', N'RL_MENU_PM', N'0001', CAST(N'2025-10-21T16:09:49.000' AS DateTime), N'PERMISSIONS', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1e-752a-a5db-7285f0041b34', N'EMP_PRJ', N'0001', CAST(N'2025-10-21T16:28:25.000' AS DateTime), N'ASSIGN', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1e-758a-a8ae-217538280a16', N'VER', N'0001', CAST(N'2025-10-21T16:30:25.000' AS DateTime), N'VERIFICATION', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1e-758d-ae25-311a82537103', N'PJ', N'0002', CAST(N'2025-11-02T20:52:09.037' AS DateTime), N'PROJECT', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1e-75a0-acb8-1cbaac802e6e', N'MI', N'0001', CAST(N'2025-10-21T15:53:03.000' AS DateTime), N'MENU', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1e-7ee1-a142-010156b9e736', N'MG', N'0001', CAST(N'2025-10-21T15:52:40.000' AS DateTime), N'MENU', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a060b-cb1e-7f01-9ca3-ee6128387b1d', N'RULE', N'0001', CAST(N'2025-10-21T16:33:01.000' AS DateTime), N'RULE', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a063b-a6b4-71d4-b89e-86bc4e5cdd85', N'TASK', N'0002', CAST(N'2025-11-01T12:59:02.600' AS DateTime), N'BACKLOG', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a063b-a6b4-72a7-9f64-f68d2d56173c', N'EMP', N'0001', CAST(N'2025-10-21T16:36:32.000' AS DateTime), N'TABLE', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a063b-a6b4-74e3-be8b-3f128f07c62a', N'LOC', N'0001', CAST(N'2025-10-21T16:37:26.000' AS DateTime), N'TABLE', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a063b-a6b4-763f-ba8f-dd2f8de0ab29', N'PAY', N'0001', CAST(N'2025-10-21T16:52:36.000' AS DateTime), N'PAYROLL', 0)
INSERT [dbo].[Tbl_Sequence] ([SequenceId], [UniqueName], [SequenceNo], [SequenceDate], [SequenceType], [DeleteFlag]) VALUES (N'019a063b-a6b4-7bc1-9bde-248cb15d8d63', N'ATT', N'0001', CAST(N'2025-10-21T16:38:16.000' AS DateTime), N'ATTENDANCE', 0)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Atte__013780A2C286D91E]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Attendance] ADD UNIQUE NONCLUSTERED 
(
	[AttendanceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Comp__1893361320CFD52E]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_CompanyRule] ADD UNIQUE NONCLUSTERED 
(
	[CompanyRuleCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Empl__1F642548D8A5E86F]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Employee] ADD UNIQUE NONCLUSTERED 
(
	[EmployeeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Empl__51A84C46E17E83C9]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_EmployeeProject] ADD UNIQUE NONCLUSTERED 
(
	[EmployeeProjectCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Loca__DDB144D5BB8E821C]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Location] ADD UNIQUE NONCLUSTERED 
(
	[LocationCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Menu__868A3A73598582D2]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Menu] ADD UNIQUE NONCLUSTERED 
(
	[MenuCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Menu__22599E845A616F2F]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_MenuGroup] ADD UNIQUE NONCLUSTERED 
(
	[MenuGroupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Payr__EA6E0CAC50A52684]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Payroll] ADD UNIQUE NONCLUSTERED 
(
	[PayrollCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Proj__2F3A4948AECAA9DA]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Project] ADD UNIQUE NONCLUSTERED 
(
	[ProjectCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Role__D62CB59C8AA235C5]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Role] ADD UNIQUE NONCLUSTERED 
(
	[RoleCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Role__AB0007C896ABC595]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_RoleAndMenuPermission] ADD UNIQUE NONCLUSTERED 
(
	[RoleAndMenuPermissionCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Task__251D06996B0F73EF]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Task] ADD UNIQUE NONCLUSTERED 
(
	[TaskCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Tbl_Veri__DA24CB14A1F99E79]    Script Date: 11/2/2025 9:37:52 PM ******/
ALTER TABLE [dbo].[Tbl_Verification] ADD UNIQUE NONCLUSTERED 
(
	[VerificationCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tbl_Attendance] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_CompanyRule] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Tbl_CompanyRule] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_Employee] ADD  DEFAULT ((1)) FOR [IsFirstTimeLogin]
GO
ALTER TABLE [dbo].[Tbl_Employee] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_Location] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_Menu] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_MenuGroup] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_Project] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_RefreshToken] ADD  DEFAULT ((0)) FOR [IsRevoked]
GO
ALTER TABLE [dbo].[Tbl_Role] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[Tbl_Verification] ADD  DEFAULT ((0)) FOR [DeleteFlag]
GO
/****** Object:  StoredProcedure [dbo].[sp_GenerateCode]    Script Date: 11/2/2025 9:37:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GenerateCode] @CodePrefix NVARCHAR(50),
                                 @GeneratedCode NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TodayDate NVARCHAR(8) = CONVERT(NVARCHAR(8), GETDATE(), 12); -- yymmdd
    DECLARE @CurrentSeqNo INT;
    DECLARE @NewSeqNo NVARCHAR(50);
    DECLARE @UniqueName NVARCHAR(100);
    DECLARE @StoredDate DATE;

    -- Determine UniqueName
--     IF @CodePrefix = 'TASK' AND @ProjectCode IS NOT NULL
--         SET @UniqueName = 'TASK_' + @ProjectCode;
--     ELSE
    SET @UniqueName = @CodePrefix;

    -- Get current sequence and stored date
    SELECT @CurrentSeqNo = TRY_CAST(SequenceNo AS INT),
           @StoredDate = CAST(SequenceDate AS DATE)
    FROM Tbl_Sequence
    WHERE UniqueName = @UniqueName
      AND DeleteFlag = 0;

    -- If no row exists, return NULL
    IF @CurrentSeqNo IS NULL OR @StoredDate IS NULL
        BEGIN
            SET @GeneratedCode = NULL;
            RETURN;
        END

    -- Reset if date is older than today
    IF @StoredDate < CAST(GETDATE() AS DATE)
        SET @CurrentSeqNo = 1;
    ELSE
        SET @CurrentSeqNo = @CurrentSeqNo + 1;

    -- Format sequence number
    SET @NewSeqNo = RIGHT('0000' + CAST(@CurrentSeqNo AS NVARCHAR(50)), 4);

    -- Update sequence
    UPDATE Tbl_Sequence
    SET SequenceNo   = @NewSeqNo,
        SequenceDate = GETDATE()
    WHERE UniqueName = @UniqueName
      AND DeleteFlag = 0;

    -- Build final code
--     IF @CodePrefix = 'TASK' AND @ProjectCode IS NOT NULL
--         SET @GeneratedCode = 'TASK_' + @ProjectCode + '_' + @NewSeqNo;
--     ELSE
        SET @GeneratedCode = @CodePrefix + '_' + @TodayDate + '_' + @NewSeqNo;
END

GO
INSERT INTO Tbl_CompanyRule 
(CompanyRuleId, CompanyRuleCode, Description, Value, IsActive, CreatedAt, CreatedBy, DeleteFlag)
VALUES
(NEWID(), 'OFFICE_START_TIME', 'Office start time', '09:00', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'OFFICE_END_TIME', 'Office end time', '17:00', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'OFFICE_BREAK_HOUR', 'Total break duration (hours)', '1', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'CHECKIN_ACCEPTABLE', 'Check-in acceptable until', '09:30', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'CHECKIN_ONE_HOUR_LATE', 'Check-in one hour late threshold', '10:00', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'CHECKOUT_ACCEPTABLE', 'Checkout acceptable after', '16:30', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'CHECKOUT_HOURLATE', 'Checkout hour-late threshold', '16:00', 1, GETDATE(), 'SYSTEM', 0),

-- Half Day / Morning Half Rules
(NEWID(), 'MORNING_HALF_CHECKIN_ACCEPTABLE', 'Morning half allowable check-in', '13:30', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'MORNING_HALF_CHECKIN_HOURLATE', 'Morning half hour-late threshold', '14:00', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'EVENING_HALF_CHECKOUT_ACCEPTABLE', 'Evening half checkout acceptable', '12:30', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'EVENING_HALF_CHECKOUT_HOURLATE', 'Evening half hour-late checkout', '12:00', 1, GETDATE(), 'SYSTEM', 0),

-- Deduction Rules
(NEWID(), 'HOUR_LATE_FLAG_DEDUCTION', 'Hour late penalty (percentage or rule)', NULL, 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'HALFDAY_FLAG_DEDUCTION', 'Half day deduction percentage', '50%', 1, GETDATE(), 'SYSTEM', 0),

(NEWID(), 'FULLDAY_FLAG_DEDUCTION', 'Full day deduction percentage', '100%', 1, GETDATE(), 'SYSTEM', 0);
GO


/****** Object:  StoredProcedure [dbo].[sp_HRAttendanceDashboard]    Script Date: 16/11/2025 7:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_HRAttendanceDashboard]
	@Date Date, @DataView Int = 0 -- 0 for Current Day, 1 for Week, 2 for Month, 3 for Year
AS
BEGIN
	SET NOCOUNT ON;
	CREATE TABLE #WorkingDays (
    DayDate DATE NOT NULL,
);

DECLARE 
    @StartDate DATE, @EndDate DATE, @EmpCount INT = 0;

    Create Table #TmpResult (Present Int, Late Int, Absent Int, EmpCount Int)


    IF @DataView = 0 -- Current Day
    Begin
        Set @StartDate = @Date;
        Set @EndDate = DATEADD(dd, -1, @Date);
    End
    ELSE IF @DataView = 1 -- Weekly
    Begin
        Set @StartDate = DATEADD(dd, -(DATEPART(dw, @Date)-1), @Date);
        Set @EndDate = DATEADD(dd, 7-(DATEPART(dw, @Date)), @Date)
    End;
    ELSE IF @DataView = 2 -- Monthly
    Begin
        Set @StartDate = DATEADD(month, DATEDIFF(month, 0, @Date), 0);
        Set @EndDate = EOMONTH(@Date);
    End;
    ELSE IF @DataView = 3 -- Yearly
    Begin
        Set @StartDate = CAST(CONCAT('1/1/', YEAR(@Date)) as Date);
        Set @EndDate = CAST(CONCAT('12/31/', YEAR(@Date)) as Date);
    End;


    WITH DateSequence AS (
        SELECT @StartDate AS CurrentDate
        UNION ALL
        SELECT DATEADD(DAY, 1, CurrentDate)
        FROM DateSequence
        WHERE CurrentDate <= @EndDate
    )
    INSERT INTO #WorkingDays (DayDate)
    SELECT
        CurrentDate AS FullDate
    FROM DateSequence
    WHERE DATEPART(WEEKDAY, CurrentDate) NOT IN (1, 7)
    OPTION (MAXRECURSION 0);

    Select @EmpCount = Count(1) From Tbl_Employee 
        Where Isnull(DeleteFlag, 0) = 0;


    Insert Into #TmpResult
    Select Sum(Isnull(FullDayFlag, 0)) as Present, Sum(IIF(Isnull(HourLateFlag, 0) = 0,0,1)) as Late, 
        (@EmpCount - Sum(Isnull(FullDayFlag, 0)) - Sum(Isnull(HalfDayFlag, 0))) as Absent, @EmpCount    
    From Tbl_Attendance ta
    Right Join #WorkingDays wd on wd.DayDate = CAST(AttendanceDate AS DATE)
    Group By CAST(AttendanceDate AS DATE), wd.DayDate;


    Select SUM(ISNULL(Present, 0)) Present, SUM(ISNULL(Late, 0)) Late, 
        SUM(ISNULL(Absent, 0)) Absent, MAX(EmpCount) EmpCount
        From #TmpResult

    DROP Table #WorkingDays, #TmpResult;
END
GO


/****** Object:  StoredProcedure [dbo].[sp_EmpAttendanceDashboard]    Script Date: 16/11/2025 7:25:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_EmpAttendanceDashboard]
	@Year INT, @EmpCode nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	CREATE TABLE #WorkingDays (
    DayDate DATE NOT NULL,
);

DECLARE 
    @StartDate DATE, @EndDate DATE;

   
    Set @StartDate = CAST(CONCAT('1/1/', @Year) as Date);
    Set @EndDate = CAST(CONCAT('12/31/', @Year) as Date);


    WITH DateSequence AS (
        SELECT @StartDate AS CurrentDate
        UNION ALL
        SELECT DATEADD(DAY, 1, CurrentDate)
        FROM DateSequence
        WHERE CurrentDate <= @EndDate
    )
    INSERT INTO #WorkingDays (DayDate)
    SELECT
        CurrentDate AS FullDate
    FROM DateSequence
    WHERE DATEPART(WEEKDAY, CurrentDate) NOT IN (1, 7)
    OPTION (MAXRECURSION 0);

    Select wd.DayDate AttendanceDate,
        Sum(Isnull(FullDayFlag, 0)) as Present, Sum(Isnull(HourLateFlag, 0)) as Late
    Into #TmpResult
    From Tbl_Attendance ta
    Right Join #WorkingDays wd on wd.DayDate = CAST(AttendanceDate AS DATE) and ta.EmployeeCode = @EmpCode 
    Group By CAST(ta.AttendanceDate AS DATE), wd.DayDate;

    Select DATENAME(month, AttendanceDate) month, SUM(ISNULL(Present, 0)) Present, SUM(ISNULL(Late, 0)) Late 
        From #TmpResult
        Group By DATENAME(month, AttendanceDate), MONTH(AttendanceDate)
        Order By MONTH(AttendanceDate);

    DROP Table #WorkingDays, #TmpResult;
END
GO
