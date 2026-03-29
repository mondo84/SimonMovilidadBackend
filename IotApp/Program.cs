using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using IotApp.DummyHandler;
using IotApp.Hubs;
using IotApp.Middleware;
using IotApp.RealTime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ===== SWAGGER API TEST
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
{

    op.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Iot API",
        Version = "v1"
    });
});

// ===== DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ====== Services injects.
builder.Services.AddSignalR();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IAlertHub, SignalrNotifier>();

// ====== CORS
string corsConfig = "My_Cors";
string[] withorigins = ["http://localhost:3000", "http://localhost:3030"];

builder.Services.AddCors(op =>
{
    op.AddPolicy(corsConfig, policy =>
    {
        policy
        .WithOrigins(withorigins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

// Registrar este esquema por defecto que ya tiene un User asignado para que .net
// tenga con que validar el Rol y no arroje el error de esquema por defecto cuando el rol cambia.
builder.Services.AddAuthentication("ManualJwt")
    .AddScheme<AuthenticationSchemeOptions, DummyHandler>("ManualJwt", options => { });
builder.Services.AddAuthorization();

var app = builder.Build();

builder.Logging.AddConsole();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(corsConfig);
app.UseHttpsRedirection();

// ====== MIDDLEWARE
app.UseMiddleware<IotMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AlertsHub>("/ws/alerts");
app.Run();
