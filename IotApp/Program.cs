using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using IotApp.Hubs;
using IotApp.Middleware;
using IotApp.RealTime;
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
const string CorsName = "My_App_Cors";
builder.Services.AddCors(op =>
{
    op.AddPolicy(CorsName, policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// MIDDLEWARE
app.UseMiddleware<IotMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsName);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AlertsHub>("/ws/alerts");
app.Run();
