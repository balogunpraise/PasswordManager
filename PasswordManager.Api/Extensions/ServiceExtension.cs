namespace PasswordManager.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection service)
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
            return service;
        }
    }
}
