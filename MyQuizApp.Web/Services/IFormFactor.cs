
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
            ConfigureHttpClient(ConfigureHttpClient);


        services.AddRefitClient<ICategoryApi>(CreateRefitSettings).
            ConfigureHttpClient(ConfigureHttpClient);
    }

    private static RefitSettings CreateRefitSettings(IServiceProvider provider)
    {
        var token = provider.GetRequiredService<ClientStateProvider>();

        return new RefitSettings()
        {
            AuthorizationHeaderValueGetter = (_, _) => Task.FromResult(token.LoggedUser?.Token ?? "")
        };
    }

    private static void ConfigureHttpClient(HttpClient client)
    {
        client.Timeout = TimeSpan.FromSeconds(120);
        client.BaseAddress = new Uri("http://localhost:5193");
    }

}
