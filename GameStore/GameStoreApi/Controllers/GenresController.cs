using GameStore.Application.Requests;
using GameStore.Domain.Entities;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Data;
using GameStore.Services.Interfaces;
using GameStore.Services.Services;
using GameStoreApi.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilter))]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService genreService;
        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenreAsync([FromBody] CreateGenreRequest request, CancellationToken token = default)
        {
            var genre = await genreService.CreateGenreAsync(request, token);
            return CreatedAtAction(nameof(GetGenreByIdAsync), new { id=genre.Id},genre);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGenreAsync([FromBody] UpdateGenreRequest request, CancellationToken token = default)
        {
            var result = await genreService.UpdateGenreAsync(request, token);
            return (result)?NoContent(): NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteGenreAsync(Guid id, CancellationToken token= default)
        {
            var result = await genreService.DeleteGenreAsync(id, token);
            return (result) ? NoContent() : NotFound();
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGenreByIdAsync(Guid id, CancellationToken token = default)
        {
            var genre = await genreService.GetGenreByIdAsync(id, token);
            return (genre is null) ? NotFound() : Ok(genre);
        }

        [HttpGet]
        [Route("{id:guid}/genres")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGenreByParentIdAsync(Guid id, CancellationToken token = default)
        {
            var genre = await genreService.GetGenresByParentIdAsync(id, token);
            return (genre is null) ? NotFound() : Ok(genre);
        }
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetAllGenresAsync(CancellationToken token = default)
        {
            var genres = await genreService.GetAllGenresAsync(token);
            return Ok(genres);
        }

        [HttpGet]
        [Route("{id:guid}/games")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameByGenreAsync(Guid id, CancellationToken token = default)
        {
            var games = await genreService.GetGameByGenreAsync(id, token);
            return (games is null) ? NotFound() : Ok(games);
        }
    }
}
