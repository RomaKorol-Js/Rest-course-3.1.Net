using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Api.Modules;

public static class DbModule
{
    public static async Task InitialiseDatabaseAsync(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
    }
}