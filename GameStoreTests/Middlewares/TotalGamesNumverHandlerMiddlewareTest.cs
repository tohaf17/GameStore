using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using GameStore.Infrastructure.Data;
using GameStore.Domain.Entities;
using GameStoreApi.Middlewares;


namespace GameStoreTests.Middlewares
{
    public class TotalGamesNumverHandlerMiddlewareTest
    {
        [Fact]
        public async Task TotalGamesNumberHandlerMiddleware_Should_Set_Header()
        {
            var context = new DefaultHttpContext();
            var cache = new MemoryCache(new MemoryCacheOptions());

            var dbContextOptions = new DbContextOptionsBuilder<GameStoreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using var dbContext = new GameStoreContext(dbContextOptions);

            dbContext.Games.Add(new Game { Id = Guid.NewGuid(), Name = "Game 1" });
            dbContext.Games.Add(new Game { Id = Guid.NewGuid(), Name = "Game 2" });
            await dbContext.SaveChangesAsync();

            var middleware = new TotalGamesHandlerMiddleware((innerHttpContext) => Task.CompletedTask, cache);
            await middleware.InvokeAsync(context, dbContext);

            string headerKey = "x-total-numbers-of-games";
            Assert.True(context.Response.Headers.ContainsKey(headerKey));

            Assert.Equal("2", context.Response.Headers[headerKey].ToString());
        }
    }
}
