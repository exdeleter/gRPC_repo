using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

/// <summary>
/// Class extends <see cref="WebApplication"/>
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary> Execute migrations. </summary>
    /// <param name="app"> Application. </param>
    public static async void ApplyMigrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();
    }

    /// <summary> Add DB. </summary>
    /// <param name="services"> Services. </param>
    /// <param name="configuration"> Configuration. </param>
    /// <returns></returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}