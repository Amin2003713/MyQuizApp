using Microsoft.Extensions.Logging;

namespace MyQuizApp.App;


using Blazored.LocalStorage;
using Infra.Common;
using Infra.Services;
using Infra.Users;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Refit;
using Services;
#if ANDROID
using System.Security.Cryptography.X509Certificates;
using Xamarin.Android.Net;
using System.Net.Security;
#endif


public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();


			builder.Services.AddSingleton<IAppState, AppState>();


		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<ILocalStorage ,MauiLocalStorage >();
		builder.Services.AddMudServices();
		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddAuthorizationCore();
		builder.Services.AddSingleton<ClientStateProvider>();
		builder.Services.AddSingleton<AuthenticationStateProvider>(
			provider => provider.GetRequiredService<ClientStateProvider>());
		builder.Services.AddSingleton<IFormFactor, FormFactor>();

		builder.Services.AddRefitConfig(CreateRefitSettings());


		return builder.Build();
	}


	private static RefitSettings CreateRefitSettings()
    {
	    return new RefitSettings()
	    {
		           
// 		     HttpMessageHandlerFactory = () =>
// 		                                  {
// #if ANDROID
// 												var android =  new AndroidMessageHandler
//                                                   {
//                                                       ServerCertificateCustomValidationCallback =
//                                                           (_, cert, chain, sslPolicyErrors) =>
//                                                               cert?.Issuer    == "CN=localhost" ||
//                                                               sslPolicyErrors == SslPolicyErrors.None
//                                                   };
//
//                                                   return android;
// #elif IOS
//                                                   var iosHandler = new NSUrlSessionHandler();
//                                                   iosHandler.TrustOverrideForUrl = (sender, url, trust) =>
//                                                       url.StartsWith(Extentions.BaseUrl, StringComparison.OrdinalIgnoreCase);
//                                                   return iosHandler;
// #endif
// 			                                  return new HttpClientHandler();
// 		                                  }
	    };
    }

}



