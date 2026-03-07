using GameStore.Application.Requests;
using GameStore.Domain.Entities;
using GameStore.Infrastructure;
using GameStore.Infrastructure.Data;
using GameStore.Services.Interfaces;
using GameStoreApi.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilter))]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService platformService;
        public PlatformsController(IPlatformService platformService)
        {
            this.platformService = platformService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatformAsync([FromBody] CreatePlatformRequest request, CancellationToken token=default)
        {
            var platform = await platformService.CreatePlatformAsync(request, token);
            return CreatedAtAction(nameof(GetPlatformByIdAsync), new { id=platform.Id},platform);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlatformAsync([FromBody] UpdatePlatformRequest request, CancellationToken token=default)
        {
            var result = await platformService.UpdatePlatformAsync(request, token);
            return (result) ? NoContent() : NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePlatformAsync(Guid id, CancellationToken token=default)
        {
            var result = await platformService.DeletePlatformAsync(id, token);
            return (result) ? NoContent() : NotFound();
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetPlatformByIdAsync(Guid id, CancellationToken token = default)
        {
            var platform = await platformService.GetPlatformByIdAsync(id, token);
            return (platform is null) ? NotFound(): Ok(platform);
        }
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetAllPlatformsAsync(CancellationToken token=default)
        {
            var platforms = await platformService.GetAllPlatformsAsync(token);
            return Ok(platforms);
        }
        [HttpGet]
        [Route("{id:guid}/games")]
        [ResponseCache(CacheProfileName = "Default1Min")]
        public async Task<IActionResult> GetGameByPlatformAsync(Guid id, CancellationToken token = default)
        {
            var games = await platformService.GetGameByPlatformAsync(id, token);
            return (games is null)? NotFound() : Ok(games);
        }
    }
}
