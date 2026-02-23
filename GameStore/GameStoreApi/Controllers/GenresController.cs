using GameStore.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStore.Domain.Entities;
using GameStore.Infrastructure;

namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly GameStoreContext dbContext;
        public GenresController(GameStoreContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{id:guid}/games")]
        public async Task<IActionResult> GetGame(Guid id)
        {
            var games = dbContext.Games.Where(p => p.Id==id);
            return Ok(games);
        }
    }
}
