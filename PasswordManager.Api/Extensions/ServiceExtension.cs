using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PasswordManager.Api.Helpers;
using PasswordManager.Api.Security;
using PasswordManager.Api.Services;
using PasswordManager.Infrastructure.Data.Context;

namespace PasswordManager.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection service, IConfiguration config)
        {
            service.AddCors(option =>
            {
                option.AddPolicy("AllowOrigins", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
            service.AddScoped<EncryptionService>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            service.AddDbContext<ApplicationDbContext>(option => option.
                UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            service.AddAutoMapper(typeof(MappingProfiles));
            service.AddSingleton<DataProtectorString>();

            service.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return service;
        }
    }
}
