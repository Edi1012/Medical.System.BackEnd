using Medical.System.Core.Middlewares;
using Medical.System.Core.Models.Entities;
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

//builder.Services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

builder.Configuration.AddEnvironmentVariables();


// Add services to the container.
builder.Services.AddSingleton<IDatabaseResolverService, DatabaseResolverService>();
builder.Services.AddSingleton<IVaultService, VaultService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<ISupplierService, SupplierService>();
builder.Services.AddSingleton<ISupplierRepository, SupplierRepository>();


//builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITokenRepository, TokenRepository>();
//builder.Services.AddTransient<IRepository<User>, Repository<User>>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();


//string key = builder.Configuration["JwtKey"]; // Obtener la clave desde un archivo de configuración o variable de entorno
//builder.Services.AddSingleton<ITokenService>(new TokenService());
builder.Services.AddSingleton<ITokenService, TokenService>();


builder.Services.AddTransient<IGenericRepository<User>>(sp => new GenericRepository<User>(
    sp.GetRequiredService<IDatabaseResolverService>(),
    DatabaseTypes.MedicalSystem,
    "Catalogs_user"
));

builder.Services.AddTransient<IGenericRepository<RevokedToken>>(sp => new GenericRepository<RevokedToken>(
       sp.GetRequiredService<IDatabaseResolverService>(),
          DatabaseTypes.MedicalSystem,
             "RevokedTokens"
             ));

builder.Services.AddTransient<IGenericRepository<Supplier>>(sp => new GenericRepository<Supplier>(
       sp.GetRequiredService<IDatabaseResolverService>(),
          DatabaseTypes.MedicalSystem,
             "Suppliers"
             ));

//builder.Services.AddTransient<ExceptionMiddleware>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"], // Change here
                ValidAudience = builder.Configuration["Jwt:Audience"], // Change here
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Change here
            };
        });

builder.Services.AddAuthorization();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

//builder.Services.AddAutoMapper(typeof(MedicalSystemMappingProfile)); // Ajusta la clase según tu estructura



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseMiddleware<ExceptionMiddleware>();


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
