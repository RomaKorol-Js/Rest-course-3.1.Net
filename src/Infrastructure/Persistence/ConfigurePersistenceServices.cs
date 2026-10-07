using Application.Common.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(dataSource, builder =>
                builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention());

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<CardRepository>();

        services.AddScoped<ICardRepository>(provider => provider.GetRequiredService<CardRepository>());
    }
}