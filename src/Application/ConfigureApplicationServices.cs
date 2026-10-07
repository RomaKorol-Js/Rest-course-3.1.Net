using Application.Cards.Services.Abstract;
using Application.Cards.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;


namespace Application;
public static class ConfigureApplicationServices
{
 public static void AddApplicationServices(this IServiceCollection services)
 {
 services.AddScoped<ICardService, CardService>();
 }
}
