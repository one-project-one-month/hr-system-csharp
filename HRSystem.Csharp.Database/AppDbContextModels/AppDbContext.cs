﻿using System;
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
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=HRSystem;User ID=sa;Password=sasa@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Tbl_Atte__8B69261C8F5D26D5");

            entity.ToTable("Tbl_Attendance");

            entity.HasIndex(e => e.AttendanceCode, "UQ__Tbl_Atte__013780A219606370").IsUnique();

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
            entity.HasKey(e => e.CompanyRuleId).HasName("PK__Tbl_Comp__5D113C0847F0CF18");

            entity.ToTable("Tbl_CompanyRule");

            entity.HasIndex(e => e.CompanyRuleCode, "UQ__Tbl_Comp__18933613F891828E").IsUnique();

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
            entity.HasKey(e => e.EmployeeId).HasName("PK__Tbl_Empl__7AD04F11E7040EA5");

            entity.ToTable("Tbl_Employee");

            entity.HasIndex(e => e.EmployeeCode, "UQ__Tbl_Empl__1F642548751EF09B").IsUnique();

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
            entity.HasKey(e => e.EmployeeProjectId).HasName("PK__Tbl_Empl__541BC8B118FDCDE0");

            entity.ToTable("Tbl_EmployeeProject");

            entity.HasIndex(e => e.EmployeeProjectCode, "UQ__Tbl_Empl__51A84C46C804565C").IsUnique();

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
            entity.HasKey(e => e.LocationId).HasName("PK__Tbl_Loca__E7FEA4974D1AA4BF");

            entity.ToTable("Tbl_Location");

            entity.HasIndex(e => e.LocationCode, "UQ__Tbl_Loca__DDB144D55BE751E4").IsUnique();

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
            entity.HasKey(e => e.MenuId).HasName("PK__Tbl_Menu__C99ED23013FA734E");

            entity.ToTable("Tbl_Menu");

            entity.HasIndex(e => e.MenuCode, "UQ__Tbl_Menu__868A3A732DE88BC0").IsUnique();

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
            entity.HasKey(e => e.MenuGroupId).HasName("PK__Tbl_Menu__1C1D7933DC9A9F51");

            entity.ToTable("Tbl_MenuGroup");

            entity.HasIndex(e => e.MenuGroupCode, "UQ__Tbl_Menu__22599E845E9CE264").IsUnique();

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
            entity.HasKey(e => e.PayrollId).HasName("PK__Tbl_Payr__99DFC672A584DD89");

            entity.ToTable("Tbl_Payroll");

            entity.HasIndex(e => e.PayrollCode, "UQ__Tbl_Payr__EA6E0CACEBBFDA9E").IsUnique();

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
            entity.HasKey(e => e.ProjectId).HasName("PK__Tbl_Proj__761ABEF0F8881060");

            entity.ToTable("Tbl_Project");

            entity.HasIndex(e => e.ProjectCode, "UQ__Tbl_Proj__2F3A49482D8E4D08").IsUnique();

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
            entity.HasKey(e => e.RoleId).HasName("PK__Tbl_Role__8AFACE1AB6C6F8A3");

            entity.ToTable("Tbl_Role");

            entity.HasIndex(e => e.RoleCode, "UQ__Tbl_Role__D62CB59C5F553D4D").IsUnique();

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
            entity.HasKey(e => e.RoleAndMenuPermissionId).HasName("PK__Tbl_Role__E8D15B1A924F483E");

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
            entity.HasKey(e => e.SequenceId).HasName("PK__Tbl_Sequ__BAD61491A63EFBF4");

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
            entity.HasKey(e => e.TaskId).HasName("PK__Tbl_Task__7C6949B1AA538923");

            entity.ToTable("Tbl_Task");

            entity.HasIndex(e => e.TaskCode, "UQ__Tbl_Task__251D0699F3D69063").IsUnique();

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
            entity.HasKey(e => e.VerificationId).HasName("PK__Tbl_Veri__306D49076E0E5526");

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
