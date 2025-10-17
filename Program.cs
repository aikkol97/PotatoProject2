using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PotatoProject2.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=127.0.0.1;Port=3306;Database=PotatoDb;User=myuser;Password=mypass;SslMode=None;";

var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings.GetValue<string>("Key");
var jwtIssuer = jwtSettings.GetValue<string>("Issuer");

if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
    throw new Exception("JWT Key is missing or too short in appsettings.json. Add a Key with at least 32 characters.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString)
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// --- FastEndpoints + Swagger ---
builder.Services.AddFastEndpoints();

// ⚡️ Προσθήκη API Explorer για Swagger/NSwag
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument();

var app = builder.Build();

// --- Middleware ---
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi();

app.Run();
