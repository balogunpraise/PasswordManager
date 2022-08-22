using PasswordManager.Api.Extensions;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Infrastructure.Data.Repositories;
using PasswordManager.Infrastructure.Services;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.File
        (
            path: "C:\\logs\\log-.txt",
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day,
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
        ).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddIdentityService(builder.Configuration); 
builder.Services.AddScoped<IPasswordDetailsRepository, PasswordDetailsRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Application Starting");
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
