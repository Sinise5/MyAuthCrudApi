using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyAuthCrudApi.Data;
using MyAuthCrudApi.Repositories;
using MyAuthCrudApi.Services;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// ✅ Menampilkan informasi error JWT lebih detail
IdentityModelEventSource.ShowPII = true;
IdentityModelEventSource.LogCompleteSecurityArtifact = true;

// ✅ Koneksi PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<AuthService>();

// ✅ Ambil JWT Key dari konfigurasi dan lakukan decoding
var jwtKey = builder.Configuration["Jwt:Key"]?.Trim() 
    ?? throw new InvalidOperationException("JWT Key is missing in configuration.");

byte[] keyBytes;
try
{
    keyBytes = Convert.FromBase64String(jwtKey);
}
catch (FormatException)
{
    throw new InvalidOperationException("JWT Key in configuration is not a valid Base64 string.");
}

var key = new SymmetricSecurityKey(keyBytes);

// ✅ Konfigurasi Authentication dan JWT Bearer
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
            IssuerSigningKey = key
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"[ERROR] Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine("[WARNING] Unauthorized access attempt detected.");
                return Task.CompletedTask;
            }
        };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();

// ✅ Middleware untuk debugging token yang diterima
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"];
    Console.WriteLine($"[DEBUG] Token diterima: {token}");
    Console.WriteLine($"[DEBUG] JWT Key digunakan: {Convert.ToBase64String(keyBytes)}");

    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
