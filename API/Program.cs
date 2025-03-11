using API.Controllers;
using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
//true
// builder.Services.AddDbContext<StoreContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// builder.Services.AddDbContext<StoreContext>(options =>
//     options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=skinet;Trusted_Connection=True;MultipleActiveResultSets=true"));
// builder.Services.AddDbContext<StoreContext>(options =>
//     options.UseSqlServer("Server=ESLAM-AMR\\SQLEXPRESS;Database=skinet;Trusted_Connection=true;MultipleActiveResultSets=true"));
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer("Server=ESLAM-AMR\\SQLEXPRESS;Database=skinet;Trusted_Connection=True;TrustServerCertificate=True"));
// .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//AddScoped for live of http request
// builder.Services.AddScoped
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddCors();
// builder.Services.AddSingleton<IConnectionMultiplexer>(config =>{
//     var connString=builder.Configuration.GetConnectionString("Redis");
//     if(connString==null)throw new Exception("can not get redis connection string");
//     var configuration= ConfigurationOptions.Parse(connString,true);
//     return ConnectionMultiplexer.Connect(configuration);
// });
// builder.Services.AddSingleton<ICartService,CartService>();





builder.Services.AddSingleton<IConnectionMultiplexer>(config => 
{
    var connString = builder.Configuration.GetConnectionString("Redis") 
        ?? throw new Exception("Cannot get redis connection string");
    var configuration = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<ICartService, CartService>();


//before this lines is services 
var app = builder.Build();
//after this lines is middleware

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();


app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200","https://localhost:4200"));


app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
var services  =scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
//create database if not existing
await context.Database.MigrateAsync();
await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}
app.Run();
