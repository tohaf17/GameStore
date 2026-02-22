using GameStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly GameStoreContext dbContext;
        public PlatformsController(GameStoreContext dbContext)
        {
            this.dbContext= dbContext;
        }

        [HttpGet]
        [Route("{id:guid}/games")]
        public async Task<IActionResult> GetGame(Guid id)
        {
            var games = dbContext.Games.Where(p=>p.Id==id);
            return Ok(games);
        }
    }
}
