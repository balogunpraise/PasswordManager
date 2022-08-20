using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Helpers;
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
            return service;
        }
    }
}
