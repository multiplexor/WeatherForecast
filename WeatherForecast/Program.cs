using WeatherForecast.Clients.OpenMeteoClient;
using WeatherForecast.Clients.OpenWeatherMapClient;
using WeatherForecast.Interfaces;
using WeatherForecast.Services;
using WeatherForecast.Services.LocationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IOpenMeteoClient, OpenMeteoClient>();
builder.Services.AddHttpClient<IOpenWeatherMapClient, OpenWeatherMapClient>();
builder.Services.AddHttpClient<ILocationService, LocationService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IWeatherForecastService, OpenMeteoService>();
builder.Services.AddScoped<IWeatherForecastService, OpenWeatherMapService>();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
