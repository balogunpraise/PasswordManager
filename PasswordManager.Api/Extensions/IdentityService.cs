using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Core.Domain.Entities;
using PasswordManager.Infrastructure.Data.IdentityContext;
using System.Text;

namespace PasswordManager.Api.Extensions
{
    public static class IdentityService
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection service, IConfiguration config)
        {
            var builder = service.AddIdentityCore<AppUser>();
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<UserDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });
            var connectionString = config.GetConnectionString("IdentityConnection");
            service.AddDbContext<UserDbContext>(option => option.
                UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            return service;
        }
    }
}
