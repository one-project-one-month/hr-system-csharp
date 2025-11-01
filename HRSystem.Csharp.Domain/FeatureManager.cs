using HRSystem.Csharp.Domain.Features.RoleMenuPermission;
﻿using System.Data;
using HRSystem.Csharp.Domain.Features.Roles;
using Microsoft.Data.SqlClient;

namespace HRSystem.Csharp.Domain;

public static class FeatureManager
{
    private static void AddServices(this WebApplicationBuilder builder)
    {
        #region User Management BL

        builder.Services.AddScoped<BL_Role>();
        builder.Services.AddScoped<BL_Attendance>();
        builder.Services.AddScoped<BL_Menu>();
        builder.Services.AddScoped<BL_MenuGroup>();
        builder.Services.AddScoped<BL_Location>();
        builder.Services.AddScoped<BL_Project>();
        builder.Services.AddScoped<BL_Employee>();
        builder.Services.AddScoped<BL_Auth>();

        #endregion

        #region Main Nav Bar BL

        builder.Services.AddScoped<BL_Task>();

        #endregion

        #region User Management DA

        builder.Services.AddScoped<DA_Role>();
        builder.Services.AddScoped<DA_Attendance>();
        builder.Services.AddScoped<DA_Menu>();
        builder.Services.AddScoped<DA_MenuGroup>();
        builder.Services.AddScoped<DA_Location>();
        builder.Services.AddScoped<DA_Project>();
        builder.Services.AddScoped<DA_Employee>();
        builder.Services.AddScoped<DA_Auth>();

        #endregion

        #region Main Nav Bar DA

        builder.Services.AddScoped<DA_Task>();

        #endregion

        #region Role Menu Permission

        builder.Services.AddScoped<BL_RoleMenuPermission>();
        builder.Services.AddScoped<DA_RoleMenuPermission>();

        #endregion

        builder.Services.AddScoped<Generator>();
        builder.Services.AddScoped<JwtService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<AuthorizationService>();
    }

    public static void AddDomain(this WebApplicationBuilder builder)
    {
        var mssqlConnection = builder.Configuration.GetConnectionString("DbConnection");

        builder.Services.AddDbContext<AppDbContext>(opt => { opt.UseSqlServer(mssqlConnection); },
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        builder.Services.AddTransient<IDbConnection, SqlConnection>(n =>
            new SqlConnection(mssqlConnection));

        builder.AddServices();
    }
}