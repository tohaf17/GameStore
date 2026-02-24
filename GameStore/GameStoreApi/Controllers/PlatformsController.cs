using GameStore.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStore.Domain.Entities;
using GameStore.Services.Interfaces;
using GameStore.Infrastructure;
using GameStore.Application.Requests;

namespace GameStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService platformService;
        public PlatformsController(IPlatformService platformService)
        {
            this.platformService = platformService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatformAsync([FromBody] CreatePlatformRequest request, CancellationToken token)
        {
            var createdPlatformId = await platformService.CreatePlatformAsync(request, token);
            return CreatedAtAction(nameof(GetPlatformByIdAsync), new { createdPlatformId,token});
        }
        [HttpGet]
        [Route("{id:guid}/games")]
        public async Task<IActionResult> GetGameByPlatformAsync(Guid id, CancellationToken token)
        {
            var games = await platformService.GetGameByPlatformAsync(id, token);
            return Ok(games);
        }
    }
}
