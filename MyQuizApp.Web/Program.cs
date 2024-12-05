using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MyQuizApp.Infra.Common;
using MyQuizApp.Infra.Services;
using MyQuizApp.Infra.Users;
using MyQuizApp.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<MyQuizApp.Web.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<IAppState, AppState>();

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddMudServices();
builder.Services.AddMudBlazorSnackbar();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<ClientStateProvider>();
builder.Services.AddSingleton<ILocalStorage , WebLocalStorage>();
builder.Services.AddSingleton<AuthenticationStateProvider>(provider => provider.GetRequiredService<ClientStateProvider>());
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddRefitConfig();


await builder.Build().RunAsync();