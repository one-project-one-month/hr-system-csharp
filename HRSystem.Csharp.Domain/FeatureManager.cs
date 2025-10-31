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

        builder.Services.AddScoped<Generator>();
        builder.Services.AddScoped<JwtService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<AuthorizationService>();
    }

    public static void AddDomain(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        builder.AddServices();
    }
}