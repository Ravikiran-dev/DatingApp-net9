using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowCredentials();
            });
        });

        // Add the token service to the DI container
        services.AddScoped<ITokenService, TokenService>(); // Register the token service with the DI container
        services.AddScoped<IUserRepository, UserRepository>(); // Register the user repository with the DI container
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Register AutoMapper with the DI container
        return services;
    }


}
