using KristinaBot.BL.Services;
using KristinaBot.BL.Abstracts.ServicesAbstracts;
using KristinaBot.BL.Abstracts.APIClientsAbstracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<BotHostedService>();
builder.Services.AddScoped<IWeatherAPI, WeatherAPIClient>();
builder.Services.AddScoped<ICurrencyAPI, CurrencyAPIClient>();
builder.Services.AddScoped<IAiApi, AiApiClient>();
builder.Services.AddScoped<IHelpService, HelpService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICommandIdentifierService, CommandIdentifierService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
