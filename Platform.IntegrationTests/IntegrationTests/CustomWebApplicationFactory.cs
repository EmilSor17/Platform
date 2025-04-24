using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Platform.Infraestructure.Data;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      
      var descriptor = services.SingleOrDefault(
          d => d.ServiceType == typeof(DbContextOptions<PlatformContext>));

      if (descriptor != null)
        services.Remove(descriptor);

      
      services.AddDbContext<PlatformContext>(options =>
      {
        options.UseInMemoryDatabase("InMemoryDbForTesting");
      });

   
      var sp = services.BuildServiceProvider();

      using var scope = sp.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<PlatformContext>();
      db.Database.EnsureCreated();
    });
  }

  protected override IHost CreateHost(IHostBuilder builder)
  {
   
    builder.UseContentRoot(Directory.GetCurrentDirectory());
    return base.CreateHost(builder);
  }
}
