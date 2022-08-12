using PasswordManager.Api.Extensions;
using PasswordManager.Core.Application.Interfaces;
using PasswordManager.Infrastructure.Data.Repositories;
using PasswordManager.Infrastructure.Services;
using Serilog;


var logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddIdentityService(builder.Configuration);

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordDetailsRepository, PasswordDetailsRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
