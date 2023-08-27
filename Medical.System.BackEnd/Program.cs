using Medical.System.Core.Mapeo;
using Medical.System.Core.Middlewares;
using Medical.System.Core.Repositories;
using Medical.System.Core.Repositories.Implementations;
using Medical.System.Core.Repositories.Interfaces;
using Medical.System.Core.Services.Implementations;
using Medical.System.Core.Services.Interfaces;
using Medical.System.Core.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Configure services.
builder.Services.AddSingleton<IDatabaseResolverService, DatabaseResolverService>();
builder.Services.AddSingleton<IVaultService, VaultService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITokenRepository, TokenRepository>();
builder.Services.AddSingleton<ISupplierService, SupplierService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ISupplierRepository, SupplierRepository>();
builder.Services.AddTransient<IPatientRepository, PatientRepository>();
builder.Services.AddTransient<IPatientService, PatientService>();
builder.Services.AddAutoMapper(typeof(PatientProfile).Assembly);



// Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

// Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

    // Configuración para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Introduce el token JWT con 'Bearer ' al inicio",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Swagger (uncomment the environment check if needed)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

bool isAuthenticationEnabled = builder.Configuration.GetValue<bool>("Authentication:Enabled");

//if (!isAuthenticationEnabled)
//{
//    builder.Services.AddAuthorization(options =>
//    {
//        // Define tu política global
//        options.FallbackPolicy = new AuthorizationPolicyBuilder()
//            .RequireAuthenticatedUser()
//            .Build();
//    });
//}


app.UseAuthorization();
app.MapControllers();
app.Run();
