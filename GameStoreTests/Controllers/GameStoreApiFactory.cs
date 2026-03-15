using GameStore.Infrastructure.Data;
using GameStoreApi.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// Клас-заглушка, щоб DI не намагався створити Castle Proxy
public class TestExceptionMapper : IExceptionMapper
{
    public bool CanMap(Exception exception) => false;
    public ExceptionResponse Map(Exception exception) => new ExceptionResponse(500, "Test Error");
}

public class GameStoreApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Повністю видаляємо ВСІ реєстрації IExceptionMapper, 
            // які наробила бібліотека Scrutor/Castle
            var descriptors = services
                .Where(d => d.ServiceType == typeof(IExceptionMapper))
                .ToList();

            foreach (var d in descriptors)
            {
                services.Remove(d);
            }

            // 2. Додаємо нашу "чисту" заглушку (без проксі)
            // Тепер ExceptionHandlingMiddleware отримає цей список і заспокоїться
            services.AddSingleton<IExceptionMapper, TestExceptionMapper>();

            // 3. Очищення бази (твій існуючий код)
            var dbContextOptions = services
                .Where(d => d.ServiceType.Name.Contains("DbContextOptions"))
                .ToList();
            foreach (var d in dbContextOptions) services.Remove(d);

            services.AddDbContext<GameStoreContext>(options =>
                options.UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid().ToString()));
        });
    }
}