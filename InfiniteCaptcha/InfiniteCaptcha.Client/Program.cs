using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using InfiniteCaptcha.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

if (builder.HostEnvironment.IsDevelopment())
{
    builder.Services.AddScoped(sp =>
    {
        var client = new HttpClient { BaseAddress = new Uri("http://localhost:5205/") };
        client.DefaultRequestHeaders.Add("X-API-KEY", "SuperSecretKpiKey2026!");
        return client;
    });
}
else
{
    builder.Services.AddScoped(sp =>
    {
        var client = new HttpClient { BaseAddress = new Uri("https://kpi-project-i7n6.onrender.com/") };
        client.DefaultRequestHeaders.Add("X-API-KEY", "SuperSecretKpiKey2026!");
        return client;
    });
}

await builder.Build().RunAsync();