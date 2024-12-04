using MyQuizApp.Infra.Categories;
using MyQuizApp.Infra.Quiezzes;
using MyQuizApp.Infra.Users;
using Refit;

namespace MyQuizApp.Web.Services;

using System.Net.Security;
using Infra.Common;
using Infra.Services;
using Microsoft.Extensions.DependencyInjection;


public static class Extentions
{
    private static readonly string BaseUrl = OperatingSystem.IsBrowser()
        ? ("http://localhost:5193")
        : ("http://10.0.2.2:5193");

    public static void AddRefitConfig(this IServiceCollection services )
    {

        services.AddRefitClient<IUserApiClient>(CreateRefitSettings).
            ConfigureHttpClient(ConfigureHttpClient);


        services.AddRefitClient<ICategoryApi>(CreateRefitSettings).
            ConfigureHttpClient(ConfigureHttpClient);

        services.AddRefitClient<IQuizApi>(CreateRefitSettings).
                 ConfigureHttpClient(ConfigureHttpClient);
    }

    private static RefitSettings CreateRefitSettings(IServiceProvider provider)
    {
        var token = provider.GetRequiredService<ClientStateProvider>();

        return new RefitSettings()
        {
            AuthorizationHeaderValueGetter = (_, _) => Task.FromResult(token.LoggedUser?.Token ?? "") ,
            HttpMessageHandlerFactory = () =>
                                        {
                                            if (OperatingSystem.IsAndroid())
                                            {
                                                var android = new Xamarin.Android.Net.AndroidMessageHandler();
                                                android.ServerCertificateCustomValidationCallback
                                                    = (_, cert, chain, sslPolicyErrors) =>
                                                          cert.Issuer        == "CN==localhost"
                                                          || sslPolicyErrors == SslPolicyErrors.None;

                                                return android;
                                            }
                                            else if (OperatingSystem.IsIOS())
                                            {
                                                var msUilSession = new NSUrlSessionHandler();
                                                msUilSession.TrustOverrideForUrl = (sender, url, trust)
                                                        => url.StartsWith(BaseUrl);

                                                return msUilSession;
                                            }

                                            return default!;
                                        }
        };
    }

    private static void ConfigureHttpClient(HttpClient client)
    {
        
        client.Timeout = TimeSpan.FromSeconds(120);
        client.BaseAddress = new Uri(BaseUrl);

    }

}