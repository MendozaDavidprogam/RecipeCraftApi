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
using FluentValidation;
using FluentValidation.AspNetCore;
using RecipeCraftApi.Application.DTOs;
using RecipeCraftApi.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

foreach (System.Collections.DictionaryEntry pair in Environment.GetEnvironmentVariables())
{
builder.Configuration[pair.Key.ToString()] = pair.Value.ToString();
}

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(
$"Host={builder.Configuration["DB_HOST"]};" +
$"Port={builder.Configuration["DB_PORT"]};" +
$"Database={builder.Configuration["DB_NAME"]};" +
$"Username={builder.Configuration["DB_USER"]};" +
$"Password={builder.Configuration["DB_PASSWORD"]}"
)
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IIngredientTypeRepository, IngredientTypeRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IngredientTypeService>();
builder.Services.AddScoped<IngredientService>();

builder.Services.AddControllers()
.AddFluentValidation(fv => fv.AutomaticValidationEnabled = true);
builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterValidator>();
builder.Services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();
builder.Services.AddScoped<IValidator<IngredientTypeDto>, IngredientTypeValidator>();
builder.Services.AddScoped<IValidator<IngredientDto>, IngredientValidator>();

var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT_SECRET"]);
builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.RequireHttpsMetadata = true;
options.SaveToken = true;


options.Events = new JwtBearerEvents
{
    OnMessageReceived = context =>
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            context.Token = authHeader.Substring("Bearer ".Length).Trim();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecipeCraftApi", Version = "v1" });
c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
Name = "Authorization",
Type = SecuritySchemeType.ApiKey,
In = ParameterLocation.Header,
Description = "Ingrese el token JWT"
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

if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

// Middleware global de errores
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
