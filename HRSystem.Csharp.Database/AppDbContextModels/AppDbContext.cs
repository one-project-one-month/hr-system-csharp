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

    public virtual DbSet<TblLocation> TblLocations { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblMenuGroup> TblMenuGroups { get; set; }

    public virtual DbSet<TblPayroll> TblPayrolls { get; set; }

    public virtual DbSet<TblProject> TblProjects { get; set; }

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
            entity.HasKey(e => e.AttendanceId).HasName("PK__Tbl_Atte__8B69261CA3F0543A");

            entity.ToTable("Tbl_Attendance");

            entity.HasIndex(e => e.AttendanceCode, "UQ__Tbl_Atte__013780A2A6EB4216").IsUnique();

            entity.Property(e => e.AttendanceId).HasMaxLength(200);
            entity.Property(e => e.AttendanceCode).HasMaxLength(50);
            entity.Property(e => e.AttendanceDate).HasColumnType("datetime");
            entity.Property(e => e.CheckInLocation).HasMaxLength(50);
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutLocation).HasMaxLength(50);
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Remark).HasMaxLength(200);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<TblCompanyRule>(entity =>
        {
            entity.HasKey(e => e.CompanyRuleId).HasName("PK__Tbl_Comp__5D113C0855455A44");

            entity.ToTable("Tbl_CompanyRule");

            entity.HasIndex(e => e.CompanyRuleCode, "UQ__Tbl_Comp__18933613F8C79F54").IsUnique();

            entity.Property(e => e.CompanyRuleId).HasMaxLength(200);
            entity.Property(e => e.CompanyRuleCode).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Value).HasMaxLength(50);
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Tbl_Empl__7AD04F1178590E92");

            entity.ToTable("Tbl_Employee");

            entity.HasIndex(e => e.EmployeeCode, "UQ__Tbl_Empl__1F6425480A8897CF").IsUnique();

            entity.Property(e => e.EmployeeId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.ProfileImage).HasMaxLength(200);
            entity.Property(e => e.ResignDate).HasColumnType("datetime");
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsFirstTimeLogin).HasDefaultValue(true);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblEmployeeProject>(entity =>
        {
            entity.HasKey(e => e.EmployeeProjectId).HasName("PK__Tbl_Empl__541BC8B1CC27601A");

            entity.ToTable("Tbl_EmployeeProject");

            entity.HasIndex(e => e.EmployeeProjectCode, "UQ__Tbl_Empl__51A84C46FD5F193C").IsUnique();

            entity.Property(e => e.EmployeeProjectId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmployeeProjectCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Tbl_Loca__E7FEA49733E779E9");

            entity.ToTable("Tbl_Location");

            entity.HasIndex(e => e.LocationCode, "UQ__Tbl_Loca__DDB144D586ACD589").IsUnique();

            entity.Property(e => e.LocationId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Latitude).HasMaxLength(50);
            entity.Property(e => e.LocationCode).HasMaxLength(50);
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Radius).HasMaxLength(50);
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Tbl_Menu__C99ED230EC0293C4");

            entity.ToTable("Tbl_Menu");

            entity.HasIndex(e => e.MenuCode, "UQ__Tbl_Menu__868A3A736AC886C4").IsUnique();

            entity.Property(e => e.MenuId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Icon).HasMaxLength(200);
            entity.Property(e => e.MenuCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.MenuName).HasMaxLength(200);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(200);
        });

        modelBuilder.Entity<TblMenuGroup>(entity =>
        {
            entity.HasKey(e => e.MenuGroupId).HasName("PK__Tbl_Menu__1C1D79330F8F4722");

            entity.ToTable("Tbl_MenuGroup");

            entity.HasIndex(e => e.MenuGroupCode, "UQ__Tbl_Menu__22599E84E5C3FC37").IsUnique();

            entity.Property(e => e.MenuGroupId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Icon).HasMaxLength(200);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupName).HasMaxLength(200);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(200);
        });

        modelBuilder.Entity<TblPayroll>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Tbl_Payr__99DFC6722E3D1690");

            entity.ToTable("Tbl_Payroll");

            entity.HasIndex(e => e.PayrollCode, "UQ__Tbl_Payr__EA6E0CACC69E033D").IsUnique();

            entity.Property(e => e.PayrollId).HasMaxLength(200);
            entity.Property(e => e.ActualWorkingHour).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Allowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bonus).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Deduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.GrossPay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LeaveHour).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.NetPay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PayrollCode).HasMaxLength(50);
            entity.Property(e => e.PayrollDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Tbl_Proj__761ABEF0AE3B4EE5");

            entity.ToTable("Tbl_Project");

            entity.HasIndex(e => e.ProjectCode, "UQ__Tbl_Proj__2F3A49480454BF7E").IsUnique();

            entity.Property(e => e.ProjectId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.ProjectDescription).HasMaxLength(200);
            entity.Property(e => e.ProjectName).HasMaxLength(200);
            entity.Property(e => e.ProjectStatus).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Tbl_Role__8AFACE1A798C8D4B");

            entity.ToTable("Tbl_Role");

            entity.HasIndex(e => e.RoleCode, "UQ__Tbl_Role__D62CB59C8451DC87").IsUnique();

            entity.Property(e => e.RoleId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(200);
            entity.Property(e => e.UniqueName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRoleAndMenuPermission>(entity =>
        {
            entity.HasKey(e => e.RoleAndMenuPermissionId).HasName("PK__Tbl_Role__E8D15B1AE9821F2A");

            entity.ToTable("Tbl_RoleAndMenuPermission");

            entity.HasIndex(e => e.RoleAndMenuPermissionCode, "UQ__Tbl_Role__AB0007C8DA17F655").IsUnique();

            entity.Property(e => e.RoleAndMenuPermissionId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.MenuCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.RoleAndMenuPermissionCode).HasMaxLength(50);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblSequence>(entity =>
        {
            entity.HasKey(e => e.SequenceId).HasName("PK__Tbl_Sequ__BAD61491B27105C2");

            entity.ToTable("Tbl_Sequence");

            entity.Property(e => e.SequenceId).HasMaxLength(200);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.SequenceDate).HasColumnType("datetime");
            entity.Property(e => e.SequenceNo).HasMaxLength(50);
            entity.Property(e => e.SequenceType).HasMaxLength(50);
            entity.Property(e => e.UniqueName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tbl_Task__7C6949B16C8A281C");

            entity.ToTable("Tbl_Task");

            entity.HasIndex(e => e.TaskCode, "UQ__Tbl_Task__251D069986D78D07").IsUnique();

            entity.Property(e => e.TaskId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TaskCode).HasMaxLength(50);
            entity.Property(e => e.TaskDescription).HasMaxLength(200);
            entity.Property(e => e.TaskName).HasMaxLength(50);
            entity.Property(e => e.TaskStatus).HasMaxLength(50);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<TblVerification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PK__Tbl_Veri__306D4907CD7C6330");

            entity.ToTable("Tbl_Verification");

            entity.HasIndex(e => e.VerificationCode, "UQ__Tbl_Veri__DA24CB1409C6730C").IsUnique();

            entity.Property(e => e.VerificationId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.ExpiredTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.VerificationCode).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
