using GameStore.Domain.Entities;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Data;
using GameStore.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStore.Services.Interfaces;
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

        
        [HttpGet]
        [Route("{id:guid}/games")]
        public async Task<IActionResult> GetGameByGenreAsync(Guid id, CancellationToken token)
        {
            var games = await genreService.GetGameByGenreAsync(id, token);
            return Ok(games);
        }
    }
}
