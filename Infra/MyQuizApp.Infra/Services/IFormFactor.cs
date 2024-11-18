using Microsoft.Extensions.DependencyInjection;
using MyQuizApp.Infra.Users;
using Refit;

namespace MyQuizApp.Infra.Services;

public interface IFormFactor
{
    public string GetFormFactor();
    public string GetPlatform();
}



public static class Extentions
{
    public static void AddRefitConfig(this IServiceCollection services)
    {
        services.AddRefitClient<IAuthClient>().ConfigureHttpClient(client =>
        { 
            client.Timeout = TimeSpan.FromSeconds(120);
            client.BaseAddress = new Uri("https://localhost:7296");
        });
    }
}