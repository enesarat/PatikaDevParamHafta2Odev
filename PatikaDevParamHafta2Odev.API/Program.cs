using Microsoft.EntityFrameworkCore;
using PatikaDevParamHafta2Odev.API.Helper;
using PatikaDevParamHafta2Odev.Business.Abstract;
using PatikaDevParamHafta2Odev.Business.Concrete;
using PatikaDevParamHafta2Odev.DataAccess.Abstract;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Context;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.EntityFramework;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("appDatabase");
});
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<IProductsService,ProductsManager>();
builder.Services.AddScoped<IProductsDAL, EfProductsRepository>();




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

app.Run();
