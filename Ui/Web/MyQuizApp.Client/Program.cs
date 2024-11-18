using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyQuizApp.Client.Services;
using MyQuizApp.Components.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the MyQuizApp.App.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();
