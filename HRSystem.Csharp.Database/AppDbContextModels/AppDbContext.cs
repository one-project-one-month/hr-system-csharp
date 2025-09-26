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

    public virtual DbSet<TblAttendanceSetting> TblAttendanceSettings { get; set; }

    public virtual DbSet<TblClientSite> TblClientSites { get; set; }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    public virtual DbSet<TblEmployeeShift> TblEmployeeShifts { get; set; }

    public virtual DbSet<TblMenuGroup> TblMenuGroups { get; set; }

    public virtual DbSet<TblMenuItem> TblMenuItems { get; set; }

    public virtual DbSet<TblPayroll> TblPayrolls { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblSequence> TblSequences { get; set; }

    public virtual DbSet<TblTask> TblTasks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Tbl_Atte__8B69263CEA38E1E1");

            entity.ToTable("Tbl_Attendance");

            entity.Property(e => e.AttendanceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AttendanceID");
            entity.Property(e => e.CheckInLocation).HasMaxLength(20);
            entity.Property(e => e.CheckOutLocation).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.EmployeeShiftCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.OvertimeHour).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<TblAttendanceSetting>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PK__Tbl_Atte__C0A838E17B1C0440");

            entity.ToTable("Tbl_AttendanceSetting");

            entity.Property(e => e.ShiftId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ShiftID");
            entity.Property(e => e.ShiftCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblClientSite>(entity =>
        {
            entity.HasKey(e => e.ClientSiteId).HasName("PK__Tbl_Clie__FF11825F1B12181F");

            entity.ToTable("Tbl_ClientSites");

            entity.Property(e => e.ClientSiteId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientSiteCode).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Latitude).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Radius).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Tbl_Empl__7AD04F11CDC743E8");

            entity.ToTable("Tbl_Employee");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccName).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmployeeName).HasMaxLength(200);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.ProfileImage).HasMaxLength(200);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblEmployeeShift>(entity =>
        {
            entity.HasKey(e => e.EmployeeShiftId).HasName("PK__Tbl_Empl__2FBBBA3348EC1B92");

            entity.ToTable("Tbl_EmployeeShift");

            entity.Property(e => e.EmployeeShiftId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmployeeShiftCode).HasMaxLength(50);
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftCode).HasMaxLength(50);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblMenuGroup>(entity =>
        {
            entity.HasKey(e => e.MenuGroupId).HasName("PK__Tbl_Menu__1C1D79330D713F98");

            entity.ToTable("Tbl_MenuGroup");

            entity.Property(e => e.MenuGroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupName).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(100);
        });

        modelBuilder.Entity<TblMenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("PK__Tbl_Menu__8943F72267B6EEFD");

            entity.ToTable("Tbl_MenuItem");

            entity.Property(e => e.MenuItemId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.MenuItemCode).HasMaxLength(50);
            entity.Property(e => e.MenuItemName).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(100);
        });

        modelBuilder.Entity<TblPayroll>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Tbl_Payr__99DFC67266DBCFE0");

            entity.ToTable("Tbl_Payroll");

            entity.Property(e => e.PayrollId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Allowance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AttendenceDeduction).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Bonus).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.OvertimePay).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus).HasMaxLength(15);
            entity.Property(e => e.PayrollCode).HasMaxLength(50);
            entity.Property(e => e.Tax).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalLeaveDays).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalOvertimeHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalSalary).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Tbl_Perm__EFA6FB2F1BDD4139");

            entity.ToTable("Tbl_Permissions");

            entity.Property(e => e.PermissionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.MenuCode).HasMaxLength(50);
            entity.Property(e => e.MenuGroupCode).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.PermissionCode).HasMaxLength(50);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Tbl_Role__8AFACE1AE00AFBA2");

            entity.ToTable("Tbl_Role");

            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(200);
        });

        modelBuilder.Entity<TblSequence>(entity =>
        {
            entity.HasKey(e => e.SequenceId).HasName("PK__Tbl_Sequ__BAD61491F29338A6");

            entity.ToTable("Tbl_Sequence");

            entity.Property(e => e.SequenceId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.SequenceDate).HasColumnType("datetime");
            entity.Property(e => e.SequenceNo).HasMaxLength(50);
            entity.Property(e => e.SequenceType).HasMaxLength(50);
            entity.Property(e => e.UniqueName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tbl_Task__7C6949D1D193548B");

            entity.ToTable("Tbl_Tasks");

            entity.Property(e => e.TaskId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TaskID");
            entity.Property(e => e.Assignee).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaskCode).HasMaxLength(50);
            entity.Property(e => e.TaskName).HasMaxLength(50);
            entity.Property(e => e.WorkingHour).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
