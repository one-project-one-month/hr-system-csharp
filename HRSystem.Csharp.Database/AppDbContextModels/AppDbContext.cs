using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAttendance> TblAttendances { get; set; }

    public virtual DbSet<TblCompanyRule> TblCompanyRules { get; set; }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    public virtual DbSet<TblEmployeeProject> TblEmployeeProjects { get; set; }

    public virtual DbSet<TblHoliday> TblHolidays { get; set; }

    public virtual DbSet<TblLeave> TblLeaves { get; set; }

    public virtual DbSet<TblLocation> TblLocations { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblMenuGroup> TblMenuGroups { get; set; }

    public virtual DbSet<TblPayroll> TblPayrolls { get; set; }

    public virtual DbSet<TblPayrollSummary> TblPayrollSummaries { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblProject> TblProjects { get; set; }

    public virtual DbSet<TblRefreshToken> TblRefreshTokens { get; set; }

    public virtual DbSet<TblResetPasswordToken> TblResetPasswordTokens { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRoleAndMenuPermission> TblRoleAndMenuPermissions { get; set; }

    public virtual DbSet<TblSequence> TblSequences { get; set; }

    public virtual DbSet<TblTask> TblTasks { get; set; }

    public virtual DbSet<TblVerification> TblVerifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=HRSystem;User ID=sa;Password=sasa@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Tbl_Atte__8B69261CED479456");

            entity.ToTable("Tbl_Attendance");

            entity.HasIndex(e => e.AttendanceCode, "UQ__Tbl_Atte__013780A26240D576").IsUnique();

            entity.Property(e => e.AttendanceId).HasMaxLength(200);
            entity.Property(e => e.AttendanceCode).HasMaxLength(50);
            entity.Property(e => e.CheckInLocation).HasMaxLength(50);
            entity.Property(e => e.CheckOutLocation).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Remark).HasMaxLength(200);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<TblCompanyRule>(entity =>
        {
            entity.HasKey(e => e.CompanyRuleId).HasName("PK__Tbl_Comp__5D113C082D784E81");

            entity.ToTable("Tbl_CompanyRule");

            entity.HasIndex(e => e.CompanyRuleCode, "UQ__Tbl_Comp__18933613B32658DE").IsUnique();

            entity.Property(e => e.CompanyRuleId).HasMaxLength(200);
            entity.Property(e => e.CompanyRuleCode).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Value).HasMaxLength(50);
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Tbl_Empl__7AD04F113081D5FC");

            entity.ToTable("Tbl_Employee");

            entity.HasIndex(e => e.EmployeeCode, "UQ__Tbl_Empl__1F64254853AB9683").IsUnique();

            entity.Property(e => e.EmployeeId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.IsFirstTimeLogin).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.ProfileImage).HasMaxLength(200);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Username).HasMaxLength(200);
        });

        modelBuilder.Entity<TblEmployeeProject>(entity =>
        {
            entity.HasKey(e => e.EmployeeProjectId).HasName("PK__Tbl_Empl__541BC8B16F8DF6ED");

            entity.ToTable("Tbl_EmployeeProject");

            entity.HasIndex(e => e.EmployeeProjectCode, "UQ__Tbl_Empl__51A84C462DFC4189").IsUnique();

            entity.Property(e => e.EmployeeProjectId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmployeeProjectCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblHoliday>(entity =>
        {
            entity.HasKey(e => e.HolidayId).HasName("PK__Tbl_Holi__2D35D57A874B3237");

            entity.ToTable("Tbl_Holiday");

            entity.Property(e => e.HolidayId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.IsWorkingHoliday).HasDefaultValue(false);
        });

        modelBuilder.Entity<TblLeave>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__Tbl_Leav__796DB95990EFBDD4");

            entity.ToTable("Tbl_Leave");

            entity.Property(e => e.LeaveId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedAt).HasColumnType("datetime");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeleteFlag).HasDefaultValue(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LeaveType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalHours).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TblLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Tbl_Loca__E7FEA497495F6582");

            entity.ToTable("Tbl_Location");

            entity.HasIndex(e => e.LocationCode, "UQ__Tbl_Loca__DDB144D53899B17C").IsUnique();

            entity.Property(e => e.LocationId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Latitude).HasMaxLength(50);
            entity.Property(e => e.LocationCode).HasMaxLength(50);
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Radius).HasMaxLength(50);
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Tbl_Menu__C99ED2306E51A984");

            entity.ToTable("Tbl_Menu");

            entity.HasIndex(e => e.MenuCode, "UQ__Tbl_Menu__868A3A73B9F5D573").IsUnique();

            entity.Property(e => e.MenuId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Icon).HasMaxLength(200);
            entity.Property(e => e.MenuCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.MenuName).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(200);
        });

        modelBuilder.Entity<TblMenuGroup>(entity =>
        {
            entity.HasKey(e => e.MenuGroupId).HasName("PK__Tbl_Menu__1C1D793390456906");

            entity.ToTable("Tbl_MenuGroup");

            entity.HasIndex(e => e.MenuGroupCode, "UQ__Tbl_Menu__22599E841E7123BB").IsUnique();

            entity.Property(e => e.MenuGroupId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Icon).HasMaxLength(200);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupName).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(200);
        });

        modelBuilder.Entity<TblPayroll>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Tbl_Payr__99DFC672D63E82A8");

            entity.ToTable("Tbl_Payroll");

            entity.Property(e => e.PayrollId).HasMaxLength(200);
            entity.Property(e => e.ActualWorkingHour).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Allowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bonus).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Deduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.GrossPay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LeaveHour).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.NetPay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PayrollCode).HasMaxLength(50);
            entity.Property(e => e.PayrollMonth).HasMaxLength(20);
            entity.Property(e => e.PayrollSummaryCode).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalWorkingHour).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<TblPayrollSummary>(entity =>
        {
            entity.HasKey(e => e.PayrollSummaryId).HasName("PK__Tbl_Payr__F2CE6A3927FD2748");

            entity.ToTable("Tbl_PayrollSummary");

            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.PayrollMonth).HasMaxLength(20);
            entity.Property(e => e.PayrollSummaryCode).HasMaxLength(50);
            entity.Property(e => e.TotalActualWorkingHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalLeaveHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalNetPay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalWorkingHours).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Tbl_Perm__EFA6FB2FDC960496");

            entity.ToTable("Tbl_Permission");

            entity.HasIndex(e => e.PermissionCode, "UQ_Permission_Code").IsUnique();

            entity.Property(e => e.PermissionId).HasMaxLength(200);
            entity.Property(e => e.PermissionCode).HasMaxLength(50);
            entity.Property(e => e.PermissionName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Tbl_Proj__761ABEF08F5C5D81");

            entity.ToTable("Tbl_Project");

            entity.HasIndex(e => e.ProjectCode, "UQ__Tbl_Proj__2F3A49488200099C").IsUnique();

            entity.Property(e => e.ProjectId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.ProjectDescription).HasMaxLength(200);
            entity.Property(e => e.ProjectName).HasMaxLength(200);
            entity.Property(e => e.ProjectStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Token).HasName("PK__Tbl_Refr__1EB4F816563CE361");

            entity.ToTable("Tbl_RefreshToken");

            entity.Property(e => e.Token).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.JwtId).HasMaxLength(450);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
        });

        modelBuilder.Entity<TblResetPasswordToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Rese__3214EC0707F5C0D5");

            entity.ToTable("Tbl_ResetPasswordToken");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(100);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Tbl_Role__8AFACE1A44F51E3A");

            entity.ToTable("Tbl_Role");

            entity.HasIndex(e => e.RoleCode, "UQ__Tbl_Role__D62CB59CC43DADB0").IsUnique();

            entity.Property(e => e.RoleId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(200);
        });

        modelBuilder.Entity<TblRoleAndMenuPermission>(entity =>
        {
            entity.HasKey(e => e.RoleAndMenuPermissionId).HasName("PK__Tbl_Role__E8D15B1A5CC2828B");

            entity.ToTable("Tbl_RoleAndMenuPermission");

            entity.HasIndex(e => new { e.RoleAndMenuPermissionCode, e.MenuGroupCode, e.MenuCode, e.PermissionCode }, "UQ_RoleMenu_Group").IsUnique();

            entity.Property(e => e.RoleAndMenuPermissionId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.MenuCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.PermissionCode).HasMaxLength(50);
            entity.Property(e => e.RoleAndMenuPermissionCode).HasMaxLength(50);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblSequence>(entity =>
        {
            entity.HasKey(e => e.SequenceId).HasName("PK__Tbl_Sequ__BAD61491A866091B");

            entity.ToTable("Tbl_Sequence");

            entity.Property(e => e.SequenceId).HasMaxLength(200);
            entity.Property(e => e.SequenceDate).HasColumnType("datetime");
            entity.Property(e => e.SequenceNo).HasMaxLength(50);
            entity.Property(e => e.SequenceType).HasMaxLength(50);
            entity.Property(e => e.UniqueName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tbl_Task__7C6949B1CE329034");

            entity.ToTable("Tbl_Task");

            entity.HasIndex(e => e.TaskCode, "UQ__Tbl_Task__251D0699517AA42E").IsUnique();

            entity.Property(e => e.TaskId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.TaskCode).HasMaxLength(50);
            entity.Property(e => e.TaskDescription).HasMaxLength(200);
            entity.Property(e => e.TaskName).HasMaxLength(50);
            entity.Property(e => e.TaskStatus).HasMaxLength(50);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<TblVerification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PK__Tbl_Veri__306D4907CCF1A0C0");

            entity.ToTable("Tbl_Verification");

            entity.HasIndex(e => e.VerificationCode, "UQ__Tbl_Veri__DA24CB1471918770").IsUnique();

            entity.Property(e => e.VerificationId).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.ExpiredTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.VerificationCode).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
