using WeatherForecast.Clients;
using WeatherForecast.Clients.OpenMeteoClient;
using WeatherForecast.Clients.OpenWeatherMapClient;
using WeatherForecast.Services.LocationService;
using WeatherForecast.Services.WeatherService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IWeatherClient, OpenMeteoClient>();
builder.Services.AddHttpClient<IWeatherClient, OpenWeatherMapClient>();

builder.Services.AddHttpClient<ILocationService, LocationService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddSwaggerGen();

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
