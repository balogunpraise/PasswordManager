using Microsoft.EntityFrameworkCore;
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
            service.AddDbContext<ApplicationDbContext>(option => option.
                UseSqlServer(config.GetConnectionString("DefaultConnection")));
            return service;
        }
    }
}
