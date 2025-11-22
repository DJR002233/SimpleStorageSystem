using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleStorageSystem.WebAPI.Data;
using SimpleStorageSystem.WebAPI.Models.Auth;
using SimpleStorageSystem.WebAPI.Services.Auth;

namespace SimpleStorageSystem.WebAPI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InitializeServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAutoMapper(typeof(Program));
        services.AddScoped<AccountService>();
        services.AddScoped<PasswordHasher<AccountInformation>>();

        services.AddDbContext<MyDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "main");
                }));
        var jwtSettings = configuration.GetSection("Jwt");
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
                };
            });

        services.AddControllers();
        services.AddOpenApi();
        services.AddAuthorization();

        return services;
    }
}