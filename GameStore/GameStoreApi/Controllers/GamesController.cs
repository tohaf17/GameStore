using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Requests;
using GameStore.Models;
using Microsoft.EntityFrameworkCore;
namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameStoreContext dbContext;

        public GamesController(GameStoreContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody]CreateGameRequest request)
        {
            Game game = new Game()
            {
                Id = Guid.NewGuid(),
                Name = request.Game.Name,
                Key = request.Game.Key,
                Description = request.Game.Description,
                GameGenres = new List<GameGenre>(),
                GamePlatforms = new List<GamePlatform>(),
            };

            foreach (var genreId in request.Genres)
            {
                var genreExists = await dbContext.Genres.AnyAsync(g => g.Id == genreId);
                if (!genreExists) return BadRequest($"Genre ID {genreId} not found.");
                game.GameGenres.Add(new GameGenre { GameId = game.Id, GenreId = genreId });
            }

            foreach (var platformId in request.Platforms)
            {
                var platformExists = await dbContext.Platforms.AnyAsync(p => p.Id == platformId);
                if (!platformExists) return BadRequest($"Platform ID {platformId} not found.");

                game.GamePlatforms.Add(new GamePlatform { GameId = game.Id, PlatformId = platformId });
            }
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(CreateGame), new { Id = game.Id, game });

        }
        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> GetGameByKey(string key)
        {
            var game = await dbContext.Games
        .Include(g => g.GameGenres)
        .Include(g => g.GamePlatforms)
        .FirstOrDefaultAsync(g => g.Key == key);

            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            return Ok(dbContext.Games.Find(id));
        }

        

    }
}
