using HRSystem.Csharp.Domain.Features.Role;
using HRSystem.Csharp.Domain.Features.Rule;
using Microsoft.Data.SqlClient;
using HRSystem.Csharp.Domain.Features.RoleMenuPermission;
using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Domain.Features.Verification;
using System.Data;
using System.Net;
using System.Net.Mail;
using DotNetEnv;
using HRSystem.Csharp.Domain.Features.CheckInOut;

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
        builder.Services.AddScoped<BL_Sequence>();
        builder.Services.AddScoped<BL_CompanyRules>();
        builder.Services.AddScoped<BL_Verification>();
        builder.Services.AddScoped<BL_CheckInOut>();

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
        builder.Services.AddScoped<DA_Sequence>();
        builder.Services.AddScoped<DA_CompanyRules>();
        builder.Services.AddScoped<DA_Verification>();
        builder.Services.AddScoped<DA_CheckInOut>();

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
        builder.Services.AddScoped<EmailService>();
    }

    public static void AddDomain(this WebApplicationBuilder builder)
    {
        Env.Load(".env.development");

        var host = Environment.GetEnvironmentVariable("DB_HOST");
        //var port = Environment.GetEnvironmentVariable("DB_PORT");
        var db = Environment.GetEnvironmentVariable("DB_NAME");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        builder.Services.AddAuthorization(); 
        var mssqlConnection = $"Server={host};Database={db};User Id={user};Password={password};TrustServerCertificate=True";

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(mssqlConnection));
        
        /*builder.Services.AddDbContext<AppDbContext>(opt => { opt.UseSqlServer(mssqlConnection)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); },
            ServiceLifetime.Transient, 
            ServiceLifetime.Transient);*/

        builder.Services.AddTransient<IDbConnection, SqlConnection>(n =>
            new SqlConnection(mssqlConnection));

        builder.Services
            .AddFluentEmail("hrsystem.opom@gmail.com")
            .AddSmtpSender(new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                        "hrsystem.opom@gmail.com",
                        "rkjs utor bqqm diyw"),
                EnableSsl = true,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000
            });

        builder.AddServices();
    }
}