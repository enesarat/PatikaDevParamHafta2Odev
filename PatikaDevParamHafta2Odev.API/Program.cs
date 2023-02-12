using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatikaDevParamHafta2Odev.API.Helper;
using PatikaDevParamHafta2Odev.Business.Abstract;
using PatikaDevParamHafta2Odev.Business.Concrete;
using PatikaDevParamHafta2Odev.DataAccess.Abstract;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Context;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.EntityFramework;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Repository;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Microsoft.Extensions.Configuration;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Extensions;
using Microsoft.AspNetCore.HttpsPolicy;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0.0",
        Title = "PatikaDev & Param RESTful API",
        Contact = new OpenApiContact
        {
            Name = "Enes Arat",
            Url = new Uri("https://github.com/enesarat"),
            Email = "enes_arat@outlook.com"
        },
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("appDatabase");
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<IProductsService, ProductsManager>();
builder.Services.AddScoped<IProductsDAL, EfProductsRepository>();
builder.Services.AddTransient<CustomExceptionMiddleware>();

//-- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --LOGGER CONFIGURATION
var logger = new LoggerConfiguration()
// Read from appsettings.json
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File(new JsonFormatter(),
        "Logs/important-logs.json",
        restrictedToMinimumLevel: LogEventLevel.Warning)
    .WriteTo.File(new JsonFormatter(),
        "Logs/information-logs.json",
        restrictedToMinimumLevel: LogEventLevel.Information)
    // Create the actual logger
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger); builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

Log.CloseAndFlush();
//-- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCustomLogger(); // Middleware call to use on action based logging management.
app.UseCustomExceptionHandler(); // Middleware call to use on exception handling.

app.Run();
