using Microsoft.EntityFrameworkCore;
using GameStore.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace GameStoreApi.Middlewares
{
    public class TotalGamesHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMemoryCache cache;
        public TotalGamesHandlerMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            this.next = next;
            this.cache = cache;
        }
        public async Task InvokeAsync(HttpContext context, GameStoreContext db)
        {
            if (!cache.TryGetValue("TotalGamesNumber", out string? totalGamesNumber))
            {
                var count = await db.Games.CountAsync();
                totalGamesNumber = count.ToString();
                cache.Set("TotalGamesNumber", totalGamesNumber, TimeSpan.FromMinutes(1));
            }
            context.Response.Headers.Append("x-total-numbers-of-games", totalGamesNumber);
            await Task.CompletedTask;

            await next(context);

        }
    }
}
