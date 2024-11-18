using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyQuiz.App.Shared.Services;
using MyQuiz.App.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the MyQuiz.App.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();
