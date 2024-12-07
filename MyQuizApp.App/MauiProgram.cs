
namespace MyQuizApp.App;


using Microsoft.Extensions.Logging;
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
			builder.Services.AddMauiBlazorWebView();

			builder.UseMauiApp<App>().
			        ConfigureFonts(
				        fonts =>
				        {
					        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				        });

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


#if ANDROID
			builder.Services.Add_Android();
#elif IOS
		builder.Services.Add_IOS();
#elif MACCATALYST
			builder.Services.Add_Mac();
#elif WINDOWS
		builder.Services.Add_Windows();
#endif

		return builder.Build();
	}






	private static RefitSettings CreateRefitSettings() 
		=> new();


	private static void Add_Android(this IServiceCollection services)
	{
	}


	// ReSharper disable once InconsistentNaming
	private static void Add_IOS(this IServiceCollection services)
	{
	}

	private static void Add_Mac(this IServiceCollection services)
	{
	}

	private static void Add_Windows(this IServiceCollection services)
	{
	}

}



