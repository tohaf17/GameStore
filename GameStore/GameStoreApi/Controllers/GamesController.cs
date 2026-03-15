using FluentValidation;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using GameStore.Domain.Entities;
using GameStore.Infrastructure.Data;
using GameStore.Services.Interfaces;
using GameStoreApi.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilter))]
    public class GamesController : ControllerBase
    {
        private readonly IGameService gameService;

        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameRequest request, CancellationToken token=default)
        {
            var game= await gameService.CreateGameAsync(request, token);
            return CreatedAtAction(nameof(GetGameByIdAsync), new {id=game.Id},game);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGameAsync([FromBody] UpdateGameRequest request,CancellationToken token=default)
        {
            var result = await gameService.UpdateGameAsync(request, token);
            return (result) ? NoContent() : NotFound();

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteGameAsync(Guid id, CancellationToken token = default)
        {
            var result = await gameService.DeleteGameAsync(id, token);
            return (result) ? NoContent() : NotFound();
        }

        [HttpGet]
        [Route("{id}/files")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameFilesAsync(Guid id, CancellationToken token= default)
        {
            var files = await gameService.GetGameFilesAsync(id, token);
            return (files)?Ok("file downloading is started"):NotFound();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetAllGamesAsync(CancellationToken token=default)
        {
            var games = await gameService.GetAllGamesAsync(token);
            return Ok(games);
        }

        [HttpGet]
        [Route("{key}")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameByKeyAsync(string key, CancellationToken token = default)
        {
            var game = await gameService.GetGameByKeyAsync(key, token);
            return (game is null) ? NotFound():Ok(game);
        }

        [HttpGet]
        [Route("{key}/genres")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameGenresAsync(string key, CancellationToken token = default)
        {
            var genres = await gameService.GetGameGenresByKeyAsync(key, token);
            return (genres is null) ? NotFound():Ok(genres);
        }

        [HttpGet]
        [Route("{key}/platforms")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGamePlatformsAsync(string key, CancellationToken token = default)
        {
            var platforms = await gameService.GetGamePlatformsByKeyAsync(key, token);
            return (platforms is null) ? NotFound() : Ok(platforms);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameByIdAsync(Guid id, CancellationToken token = default)
        {
            var game = await gameService.GetGameByIdAsync(id, token);
            return ((game is null) ? NotFound() : Ok(game));
        }
    }
}