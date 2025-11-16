using System;
using GeoInsight.Business;
using GeoInsight.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// register Razor Pages
builder.Services.AddRazorPages();

// register HttpClient for services
builder.Services.AddHttpClient("OpenMeteo", client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/");
});
builder.Services.AddHttpClient("OpenMeteoGeo", client =>
{
    client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/");
});
builder.Services.AddHttpClient("RestCountries", client =>
{
    client.BaseAddress = new Uri("https://restcountries.com/");
});
builder.Services.AddHttpClient("CoinGecko", client =>
{
    client.BaseAddress = new Uri("https://api.coingecko.com/");
});

// register services (interfaces -> implementations)
builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddScoped<IGeoInsightService, GeoInsightService>();

var app = builder.Build();

// Enable detailed error pages in development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
