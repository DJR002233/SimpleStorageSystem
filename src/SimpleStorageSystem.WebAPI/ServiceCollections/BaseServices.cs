using Microsoft.AspNetCore.Identity;
using SimpleStorageSystem.WebAPI.Models.Tables;
using SimpleStorageSystem.WebAPI.Services;

namespace SimpleStorageSystem.WebAPI.ServiceCollections;

public static class BaseCollection
{
    public static IServiceCollection InitializeBaseServices(this IServiceCollection services)
    {
        //services.AddAutoMapper(typeof(Program));
        services.AddScoped<AuthService>();
        services.AddScoped<AccountService>();
        services.AddScoped<PasswordHasher<AccountInformation>>();

        return services;
    }
}