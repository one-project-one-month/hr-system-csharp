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
            entity.HasKey(e => e.AttendanceId).HasName("PK__Tbl_Atte__8B69261CD5FFC31B");

            entity.ToTable("Tbl_Attendance");

            entity.HasIndex(e => e.AttendanceCode, "UQ__Tbl_Atte__013780A22C99F551").IsUnique();

            entity.Property(e => e.AttendanceId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AttendanceCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AttendanceDate).HasColumnType("datetime");
            entity.Property(e => e.CheckInLocation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutLocation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<TblCompanyRule>(entity =>
        {
            entity.HasKey(e => e.CompanyRuleId).HasName("PK__Tbl_Comp__5D113C08EFFC9D97");

            entity.ToTable("Tbl_CompanyRule");

            entity.HasIndex(e => e.CompanyRuleCode, "UQ__Tbl_Comp__1893361343077491").IsUnique();

            entity.Property(e => e.CompanyRuleId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CompanyRuleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Tbl_Empl__7AD04F1151E8EDC4");

            entity.ToTable("Tbl_Employee");

            entity.HasIndex(e => e.EmployeeCode, "UQ__Tbl_Empl__1F64254852A59D94").IsUnique();

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfileImage)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ResignDate).HasColumnType("datetime");
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblEmployeeProject>(entity =>
        {
            entity.HasKey(e => e.EmployeeProjectId).HasName("PK__Tbl_Empl__541BC8B1DAF7B547");

            entity.ToTable("Tbl_EmployeeProject");

            entity.HasIndex(e => e.EmployeeProjectCode, "UQ__Tbl_Empl__51A84C466CF2B8CD").IsUnique();

            entity.Property(e => e.EmployeeProjectId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Tbl_Loca__E7FEA497ADE08EFA");

            entity.ToTable("Tbl_Location");

            entity.HasIndex(e => e.LocationCode, "UQ__Tbl_Loca__DDB144D573B871C9").IsUnique();

            entity.Property(e => e.LocationId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Latitude)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LocationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Radius)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Tbl_Menu__C99ED230D7F28B27");

            entity.ToTable("Tbl_Menu");

            entity.HasIndex(e => e.MenuCode, "UQ__Tbl_Menu__868A3A734C6ABBD8").IsUnique();

            entity.Property(e => e.MenuId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Icon)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MenuCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuGroupCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblMenuGroup>(entity =>
        {
            entity.HasKey(e => e.MenuGroupId).HasName("PK__Tbl_Menu__1C1D7933040C12E3");

            entity.ToTable("Tbl_MenuGroup");

            entity.HasIndex(e => e.MenuGroupCode, "UQ__Tbl_Menu__22599E841C3FBB82").IsUnique();

            entity.Property(e => e.MenuGroupId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Icon)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MenuGroupCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuGroupName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblPayroll>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Tbl_Payr__99DFC6722E7BF360");

            entity.ToTable("Tbl_Payroll");

            entity.HasIndex(e => e.PayrollCode, "UQ__Tbl_Payr__EA6E0CACB716A3CE").IsUnique();

            entity.Property(e => e.PayrollId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ActualWorkingHour).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Allowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bonus).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Deduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GrandTotalPayroll).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LeaveHour).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.PayrollCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PayrollDate).HasColumnType("datetime");
            entity.Property(e => e.PayrollStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPayroll).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Tbl_Proj__761ABEF0E276EE98");

            entity.ToTable("Tbl_Project");

            entity.HasIndex(e => e.ProjectCode, "UQ__Tbl_Proj__2F3A4948ECDB6D6C").IsUnique();

            entity.Property(e => e.ProjectId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectDescription)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Tbl_Role__8AFACE1ACD6BC6C2");

            entity.ToTable("Tbl_Role");

            entity.HasIndex(e => e.RoleCode, "UQ__Tbl_Role__D62CB59C984242C0").IsUnique();

            entity.Property(e => e.RoleId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UniqueName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblRoleAndMenuPermission>(entity =>
        {
            entity.HasKey(e => e.RoleAndMenuPermissionId).HasName("PK__Tbl_Role__E8D15B1A39B106B1");

            entity.ToTable("Tbl_RoleAndMenuPermission");

            entity.Property(e => e.RoleAndMenuPermissionId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MenuCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuGroupCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RoleAndMenuPermissionCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblSequence>(entity =>
        {
            entity.HasKey(e => e.SequenceId).HasName("PK__Tbl_Sequ__BAD61491FC8B941B");

            entity.ToTable("Tbl_Sequence");

            entity.Property(e => e.SequenceId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SequenceDate).HasColumnType("datetime");
            entity.Property(e => e.SequenceNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SequenceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UniqueName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tbl_Task__7C6949B12A430675");

            entity.ToTable("Tbl_Task");

            entity.HasIndex(e => e.TaskCode, "UQ__Tbl_Task__251D06990540A6D9").IsUnique();

            entity.Property(e => e.TaskId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TaskCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaskDescription)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TaskName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaskStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<TblVerification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PK__Tbl_Veri__306D49076D9473F7");

            entity.ToTable("Tbl_Verification");

            entity.Property(e => e.VerificationId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ExpiredTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
