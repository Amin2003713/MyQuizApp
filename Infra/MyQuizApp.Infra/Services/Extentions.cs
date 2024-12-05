namespace MyQuizApp.Infra.Services;

using System.Net.Security;
using Categories;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Quiezzes;
using Refit;
using Users;


public static class Extentions
{
    public static readonly string BaseUrl = OperatingSystem.IsBrowser()
        ? ("http://localhost:5193")
        : ("http://10.0.2.2:5193");

    public static void AddRefitConfig(
        this IServiceCollection services
        , RefitSettings refitSettings = null!)
    {

        try
        {
            services.AddRefitClient<IUserApiClient>(refitSettings).
                     ConfigureHttpClient(ConfigureHttpClient);


            services.AddRefitClient<ICategoryApi>(provider => SetAuth(provider, refitSettings)).
                     ConfigureHttpClient(ConfigureHttpClient);

            services.AddRefitClient<IQuizApi>(provider => SetAuth(provider, refitSettings)).
                     ConfigureHttpClient(ConfigureHttpClient);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static RefitSettings? SetAuth( IServiceProvider provider , RefitSettings? refitSettings)
    {
        var token = provider.GetRequiredService<ClientStateProvider>();

        refitSettings ??= new RefitSettings();

        refitSettings.AuthorizationHeaderValueGetter = (_, _) => Task.FromResult(token.LoggedUser?.Token ?? "");

        return refitSettings;
    }


    private static void ConfigureHttpClient(HttpClient client)
    {
        
        client.Timeout = TimeSpan.FromSeconds(120);
        client.BaseAddress = new Uri(BaseUrl);

    }

}