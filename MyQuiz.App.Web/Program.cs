using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyQuiz.App.Shared;
using MyQuiz.App.Web.Services;
using MyQuizApp.Infra.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<IFormFactor, FormFactor>();
      builder.Services.AddRefitConfig();


await builder.Build().RunAsync();