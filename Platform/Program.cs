using Microsoft.EntityFrameworkCore;
using Platform.Api.Mapping;
using Platform.Core.Interfaces;
using Platform.Infraestructure.Data;
using Platform.Infraestructure.Persistence;
using Platform.Infraestructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
  {
    Title = "Platform API",
    Version = "v1",
    Description = "API para gestión de billeteras y transferencias",
    Contact = new Microsoft.OpenApi.Models.OpenApiContact
    {
      Name = "Emil Soriano",
      Email = "emilsorianosanchez@hotmail.com"
    }
  });
});


// Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));



//DbContext
builder.Services.AddDbContext<PlatformContext>(options =>
  options.UseSqlServer(configuration.GetConnectionString("Connection")));

//Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();


//Adding cors policy
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
      builder =>
      {
        builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Platform API v1");
    
  });
}

app.UseCors("AllowAll");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }