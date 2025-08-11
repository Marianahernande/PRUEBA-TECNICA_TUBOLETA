using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;

using ECommerce.Infrastructure;                    // AppDbContext
using ECommerce.Application;                      // AssemblyMarker (MediatR/AutoMapper/Validators)
using ECommerce.Application.Abstractions;         // IAppDbContext
using ECommerce.Application.Common;               // ICurrentUser
using ECommerce.Application.Notifications;        // INotificationService
using ECommerce.Application.Pricing;              // IPricingService

using ECommerce.Infrastructure.Common;            // CurrentUser
using ECommerce.Infrastructure.Notifications;     // NotificationService
using ECommerce.Infrastructure.Pricing;           // PricingService
using ECommerce.Application.Auth;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger + botón Authorize
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ECommerce API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegue el token con prefijo **Bearer**. Ej: `Bearer eyJhbGciOi...`"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// EF Core + MySQL
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

// Mapea IAppDbContext -> AppDbContext
builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

// MediatR + AutoMapper + FluentValidation
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
builder.Services.AddAutoMapper(typeof(AssemblyMarker).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);

// JWT
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

// Utilidades y servicios de dominio
builder.Services.AddHttpContextAccessor(); // necesario para CurrentUser
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPricingService, PricingService>();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ---- Seed opcional en dev ----
using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    if (env.IsDevelopment())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DbSeeder.SeedAsync(db);
    }
}

await app.RunAsync();
