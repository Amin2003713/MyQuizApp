
using MyQuizApp.Infra.Categories;
using MyQuizApp.Infra.Services;
using MyQuizApp.Infra.Users;
using Refit;

namespace MyQuizApp.Web.Services;

public class FormFactor : IFormFactor
{
    public string GetFormFactor()
    {
        return "WebAssembly";
    }

    public string GetPlatform()
    {
        return Environment.OSVersion.ToString();
    }
}

public static class Extentions
{
    public static void AddRefitConfig(this IServiceCollection services)
    {
       

        services.AddRefitClient<IUserApiClient>().
            ConfigureHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(120);
                client.BaseAddress = new Uri("https://localhost:7296");
            });


        services.AddRefitClient<ICategoryApi>(CreateRefitSettings).
            ConfigureHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(120);
                client.BaseAddress = new Uri("https://localhost:7296");
            });
    }

    private static RefitSettings CreateRefitSettings(IServiceProvider provider)
    {
        var token = provider.GetRequiredService<ClientStateProvider>();

        return new RefitSettings()
        {
            AuthorizationHeaderValueGetter = (_, _) => Task.FromResult(token.LoggedUser?.Token ?? "")
        };
    }

}
