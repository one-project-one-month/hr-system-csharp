﻿using FluentValidation;
using HRSystem.Csharp.Domain.Features;
using HRSystem.Csharp.Domain.Features.Project;
using HRSystem.Csharp.Domain.Helpers;
using HRSystem.Csharp.Domain.Models.Project;

namespace HRSystem.Csharp.Domain
{
    public static class FeatureManager
    {
        
        private static void AddServices(this WebApplicationBuilder builder)
        {
            #region User Management BL

            builder.Services.AddScoped<BL_Role>();
            builder.Services.AddScoped<BL_Project>();

            #endregion

            #region User Management DA

            builder.Services.AddScoped<DA_Role>();
            builder.Services.AddScoped<DA_Project>();
            #endregion

            #region Request Validator
            builder.Services.AddScoped<IValidator<ProjectCreateRequestModel>, ProjectCreateValidator>();
            builder.Services.AddScoped<IValidator<ProjectUpdateRequestModel>, ProjectUpdateValidator>();
            #endregion

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
}
