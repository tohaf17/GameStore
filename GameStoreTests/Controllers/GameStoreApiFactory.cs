using GameStore.Infrastructure.Data;
using GameStoreApi.Validation;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class TestExceptionMapper : IExceptionMapper
{
    public bool CanMap(Exception exception) => false;
    public ExceptionResponse Map(Exception exception) => new ExceptionResponse(500, "Test Error");
}

public class GameStoreApiFactory /*: WebApplicationFactory<Program>*/
{
    private readonly string _dbName = "TestDatabase_" + Guid.NewGuid().ToString();

    //protected override void ConfigureWebHost(IWebHostBuilder builder)
    //{
    //    builder.ConfigureServices(services =>
    //    {
    //        var descriptors = services
    //            .Where(d => d.ServiceType == typeof(IExceptionMapper))
    //            .ToList();

    //        foreach (var d in descriptors)
    //        {
    //            services.Remove(d);
    //        }

    //        services.AddSingleton<IExceptionMapper, TestExceptionMapper>();

    //        var dbContextOptions = services
    //            .Where(d => d.ServiceType.Name.Contains("DbContextOptions"))
    //            .ToList();
    //        foreach (var d in dbContextOptions) services.Remove(d);

    //        services.AddDbContext<GameStoreContext>(options =>
    //            options.UseInMemoryDatabase(_dbName)); 
    //    });
    //}
}