using GameStore.Domain.Entities;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Data;
using GameStore.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStore.Services.Interfaces;
using GameStore.Application.Requests;
namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService genreService;
        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenreAsync([FromBody] CreateGenreRequest request, CancellationToken token)
        {
            var id = await genreService.CreateGenreAsync(request, token);
            return CreatedAtAction(nameof(id), new { id, token });
        }

        [HttpGet]
        [Route("{id:guid}/games")]
        public async Task<IActionResult> GetGameByGenreAsync(Guid id, CancellationToken token)
        {
            var games = await genreService.GetGameByGenreAsync(id, token);
            return Ok(games);
        }
    }
}
