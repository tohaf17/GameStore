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
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request,CancellationToken token)
        {
            var id = await gameService.CreateAsync(request, token);
            return CreatedAtAction(nameof(id), new { id ,token});

        }

        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> GetGameByKeyAsync(string key, CancellationToken token)
        {
            var game = await gameService.GetGameByKeyAsync(key, token);
            return ((game is null )? Ok(game) : NotFound("Game not found"));
        }

    }
}
