using Microsoft.EntityFrameworkCore;
using RecipeCraftApi.Application.Services;
using RecipeCraftApi.Domain.Interfaces;
using RecipeCraftApi.Infrastructure.Database.Context;
using RecipeCraftApi.Infrastructure.Repositories;
using RecipeCraftApi.API.Middlewares;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables del .env
Env.Load();

// Agregar variables del .env a IConfiguration
foreach (System.Collections.DictionaryEntry pair in Environment.GetEnvironmentVariables())
{
    builder.Configuration[pair.Key.ToString()] = pair.Value.ToString();
}

// Configurar DbContext (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        $"Host={builder.Configuration["DB_HOST"]};" +
        $"Port={builder.Configuration["DB_PORT"]};" +
        $"Database={builder.Configuration["DB_NAME"]};" +
        $"Username={builder.Configuration["DB_USER"]};" +
        $"Password={builder.Configuration["DB_PASSWORD"]}"
    )
);

// Repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Servicios de Aplicación
builder.Services.AddScoped<UserService>();

// JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT_SECRET"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecipeCraftApi", Version = "v1" });

    // Configuración de JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseMiddleware<JwtMiddleware>();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Rutas
app.MapControllers();

// Ejecutar la aplicación
app.Run();
