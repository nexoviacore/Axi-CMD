using AxiApi.Interfaces;
using AxiApi.Repositories;
using AxiApi.Services;
using StackExchange.Redis;
using ARMCommon.Interface;
using ARMCommon.Services;
using ARMCommon.Helpers;
using AxExtend.Interface;
using AxiApi.Exceptions;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();



Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//     .WriteTo.Console()
//     .WriteTo.File(
//         "logs/log-.txt",
//         rollingInterval: RollingInterval.Day,
//         shared: true,
//         flushToDiskInterval: TimeSpan.FromSeconds(1)
//     )
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

Log.Information("Starting Axi Api...."); 

try
{
    Log.Information("Starting Axi Api...."); 
    var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IGrammarService, GrammarService>();
//builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
//{
//    var configuration = builder.Configuration.GetConnectionString("Redis");
//    return ConnectionMultiplexer.Connect(configuration!);
//});


// Log.Logger = new LoggerConfiguration()
//     .ReadFrom.Configuration(builder.Configuration)
//     .CreateLogger();





// Add services to the container.
builder.Services.AddScoped<IGrammarBootstrapService, GrammarBootstrapService>();
builder.Services.AddScoped<IUserFavouriteService, UserFavouritesService>();
builder.Services.AddScoped<ICommandConfigService, CommandConfigService>();

builder.Services.AddScoped<IAxExtend, AxExtend.AxExtend>();
/* Repositories */
builder.Services.AddScoped<IGrammarRepository, GrammarRepository>();
builder.Services.AddScoped<IUserFavouritesRepository, UserFavouritesRepository>();
builder.Services.AddScoped<ICommandConfigRepository, CommandConfigRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); 

builder.Host.UseSerilog();



/* Controller */
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy =>
        {
            policy
                //.WithOrigins("https://localhost:44304")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseExceptionHandler(); 

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

} catch (Exception ex)
{
    Log.Fatal(ex, "Application Terminated Unexpectedly"); 
} finally
{
    Log.CloseAndFlush(); 
}