using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStore.Domain.Entities;
using GameStore.Infrastructure.Data;
using GameStore.Application.Requests;
using GameStore.Services.Interfaces;
namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService gameService;

        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameRequest request, CancellationToken token)
        {
            var id = await gameService.CreateGameAsync(request, token);
            return CreatedAtAction(nameof(id), new { id, token });

        }

        [HttpPut]
        public async Task<IActionResult> UpdateGameAsync([FromBody] UpdateGameRequest request,CancellationToken token)
        {
            var result = await gameService.UpdateGameAsync(request, token);
            return (result) ? NoContent() : NotFound("Game not found");

        }

        [HttpGet]
        [Route("{id}/files")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameFilesAsync(Guid id, CancellationToken token)
        {
            var files = await gameService.GetGameFilesAsync(id, token);
            return (files)?Ok("file downloading is started"):BadRequest("File didn`t create");
        }

        [HttpDelete]
        [Route("{id :guid}")]
        public async Task<IActionResult> DeleteGameAsync(Guid id, CancellationToken token)
        {
            var result = await gameService.DeleteGameAsync(id, token);
            return (result) ? NoContent() : NotFound("Game not found");
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetAllGamesAsync(CancellationToken token)
        {
            var games = await gameService.GetAllGamesAsync(token);
            return Ok(games);
        }

        [HttpGet]
        [Route("{key}")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameByKeyAsync(string key, CancellationToken token)
        {
            var game = await gameService.GetGameByKeyAsync(key, token);
            return ((game is null) ? Ok(game) : NotFound("Game not found"));
        }

        [HttpGet]
        [Route("{key}/genres")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameGenresAsync(string key, CancellationToken token)
        {
            var genres = await gameService.GetGameGenresByKeyAsync(key, token);
            return (genres is null) ? Ok(genres) : NotFound("Game not found");
        }

        [HttpGet]
        [Route("{key}/platforms")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGamePlatformsAsync(string key, CancellationToken token)
        {
            var platforms = await gameService.GetGamePlatformsByKeyAsync(key, token);
            return (platforms is null) ? Ok(platforms) : NotFound("Game not found");
        }

        [HttpGet]
        [Route("{id :guid}")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameByIdAsync(Guid id, CancellationToken token)
        {
            var game = await gameService.GetGameByIdAsync(id, token);
            return ((game is null) ? Ok(game) : NotFound("Game not found"));

        }
    }
}