using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSystem.Csharp.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Attendance",
                columns: table => new
                {
                    AttendanceId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    AttendanceCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckInLocation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckOutLocation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    WorkingHour = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    HourLateFlag = table.Column<int>(type: "int", nullable: true),
                    HalfDayFlag = table.Column<int>(type: "int", nullable: true),
                    FullDayFlag = table.Column<int>(type: "int", nullable: true),
                    Remark = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    IsSavedLocation = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Atte__8B69261C0A3B07C6", x => x.AttendanceId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_CompanyRule",
                columns: table => new
                {
                    CompanyRuleId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    CompanyRuleCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Value = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Comp__5D113C08F7439BD4", x => x.CompanyRuleId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    EmployeeCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    RoleCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Password = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    WrongPasswordCount = table.Column<int>(type: "int", nullable: true),
                    IsFirstTime = table.Column<bool>(type: "bit", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    PhoneNo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProfileImage = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResignDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Empl__7AD04F118F49A930", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_EmployeeProject",
                columns: table => new
                {
                    EmployeeProjectId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    EmployeeProjectCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ProjectCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EmployeeCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Empl__541BC8B1631951CF", x => x.EmployeeProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Location",
                columns: table => new
                {
                    LocationId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    LocationCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Latitude = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Longitude = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Radius = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Loca__E7FEA497194BB1D6", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Menu",
                columns: table => new
                {
                    MenuId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    MenuCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MenuGroupCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MenuName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Url = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Menu__C99ED230BEED3903", x => x.MenuId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_MenuGroup",
                columns: table => new
                {
                    MenuGroupId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    MenuGroupCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MenuGroupName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Url = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    HasMenuGroup = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Menu__1C1D793332E610F7", x => x.MenuGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Payroll",
                columns: table => new
                {
                    PayrollId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    PayrollCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EmployeeCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PayrollDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PayrollStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Allowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalWorkingHour = table.Column<int>(type: "int", nullable: true),
                    LeaveHour = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    ActualWorkingHour = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Deduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayroll = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrandTotalPayroll = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Payr__99DFC672995D079A", x => x.PayrollId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Project",
                columns: table => new
                {
                    ProjectId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    ProjectCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ProjectName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ProjectDescription = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProjectStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Proj__761ABEF0DF309E01", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Role",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    RoleCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    RoleName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    UniqueName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Role__8AFACE1A73EFB179", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_RoleAndMenuPermission",
                columns: table => new
                {
                    RoleAndMenuPermissionId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    RoleAndMenuPermissionCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    RoleCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MenuGroupCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MenuCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Role__E8D15B1A8EA0FFF5", x => x.RoleAndMenuPermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Sequence",
                columns: table => new
                {
                    SequenceId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    UniqueName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SequenceNo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    SequenceDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SequenceType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    RoleCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Sequ__BAD61491067C6021", x => x.SequenceId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Task",
                columns: table => new
                {
                    TaskId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    TaskCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EmployeeCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProjectCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TaskName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TaskDescription = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TaskStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    WorkingHour = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Task__7C6949B1348572D6", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Verification",
                columns: table => new
                {
                    VerificationId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    VerificationCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tbl_Veri__306D4907D5E2C43B", x => x.VerificationId);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Atte__013780A25B6429F8",
                table: "Tbl_Attendance",
                column: "AttendanceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Comp__18933613ECA2F868",
                table: "Tbl_CompanyRule",
                column: "CompanyRuleCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Empl__1F642548281B8957",
                table: "Tbl_Employee",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Empl__51A84C46399B6DC0",
                table: "Tbl_EmployeeProject",
                column: "EmployeeProjectCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Loca__DDB144D596CC9D6A",
                table: "Tbl_Location",
                column: "LocationCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Menu__868A3A73E31733F1",
                table: "Tbl_Menu",
                column: "MenuCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Menu__22599E84DBC06754",
                table: "Tbl_MenuGroup",
                column: "MenuGroupCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Payr__EA6E0CACC6EB74AA",
                table: "Tbl_Payroll",
                column: "PayrollCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Proj__2F3A4948A1DC2FAE",
                table: "Tbl_Project",
                column: "ProjectCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Role__D62CB59CD8D64266",
                table: "Tbl_Role",
                column: "RoleCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Tbl_Task__251D069992ACF2E8",
                table: "Tbl_Task",
                column: "TaskCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_Attendance");

            migrationBuilder.DropTable(
                name: "Tbl_CompanyRule");

            migrationBuilder.DropTable(
                name: "Tbl_Employee");

            migrationBuilder.DropTable(
                name: "Tbl_EmployeeProject");

            migrationBuilder.DropTable(
                name: "Tbl_Location");

            migrationBuilder.DropTable(
                name: "Tbl_Menu");

            migrationBuilder.DropTable(
                name: "Tbl_MenuGroup");

            migrationBuilder.DropTable(
                name: "Tbl_Payroll");

            migrationBuilder.DropTable(
                name: "Tbl_Project");

            migrationBuilder.DropTable(
                name: "Tbl_Role");

            migrationBuilder.DropTable(
                name: "Tbl_RoleAndMenuPermission");

            migrationBuilder.DropTable(
                name: "Tbl_Sequence");

            migrationBuilder.DropTable(
                name: "Tbl_Task");

            migrationBuilder.DropTable(
                name: "Tbl_Verification");
        }
    }
}
