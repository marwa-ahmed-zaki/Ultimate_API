using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ultimate_API.ApplicationDbContext;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.GenericRepository;
using Ultimate_API.RepositoryPattern.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//builder.Services.AddDbContext<UltimateContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the UnitOfWork and its repository
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Register the generic repository

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//static IHostBuilder CreateHostBuilder(string[] args) =>
//      Host.CreateDefaultBuilder(args)
//          .ConfigureServices((hostContext, services) =>
//          {
//              // Get the connection string from appsettings.json
//              var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

//              // Register AppDbContext with the connection string
//              services.AddDbContext<UltimateContext>(options =>
//                  options.UseSqlServer(connectionString));

//              services.AddScoped<IUnitOfWork, UnitOfWork>();
//          });


// Register the DbContext (replace YourDbContext with the actual name)
builder.Services.AddDbContext<UltimateContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the UnitOfWork and its repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


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
