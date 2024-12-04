using Microsoft.Extensions.Logging;

namespace MyQuizApp.App;

using Blazored.LocalStorage;
using Infra.Common;
using Infra.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Services;
using Web.Services;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
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
		AppDomain.CurrentDomain.UnhandledException += (s, e) =>
		                                                {
			                                                System.Diagnostics.Debug.WriteLine(
				                                                "********** OMG! FirstChanceException **********");
			                                                System.Diagnostics.Debug.WriteLine(e);
		                                                };
#endif

		builder.Services.AddSingleton<ILocalStorage ,MauiLocalStorage >();
		builder.Services.AddMudServices();
		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddAuthorizationCore();
		builder.Services.AddSingleton<ClientStateProvider>();
		builder.Services.AddSingleton<AuthenticationStateProvider>(
			provider => provider.GetRequiredService<ClientStateProvider>());
		builder.Services.AddSingleton<IFormFactor, FormFactor>();
		builder.Services.AddRefitConfig();


		return builder.Build();
	}
}
