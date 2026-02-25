using AutoMapper;
using GameStore.Application.DTO;
using GameStore.Domain.Entities;
using GameStore.Repositories.Interfaces;
using GameStore.Repositories.Repositories;
using GameStore.Services.Exceptions;
using GameStore.Services.Interfaces;
using System;
using GameStore.Application.Requests;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;

        public PlatformService(IPlatformRepository platformRepository, IMapper mapper)
        {
            this.platformRepository = platformRepository;
            this.mapper = mapper;
        }

        

        public async Task<Guid> CreatePlatformAsync(CreatePlatformRequest request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request);

            var platform = mapper.Map<Platform>(request.Platform);

            await platformRepository.AddPlatformAsync(platform, token);

            return platform.Id;
        }

        public async Task<bool> UpdatePlatformAsync(UpdatePlatformRequest request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request);
            Validation.ValidateGuid(request.Platform.Id, nameof(request.Platform.Id));

            var existingPlatform = await platformRepository.GetPlatformByIdAsync(request.Platform.Id, token);

            if (existingPlatform is null)
            {
                return false;
            }

            mapper.Map(request.Platform, existingPlatform);
            await platformRepository.UpdatePlatformAsync(existingPlatform, token);
            return true;
        }

        public async Task<bool> DeletePlatformAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var existingPlatform = await platformRepository.GetPlatformByIdAsync(id, token);

            if (existingPlatform is null)
            {
                return false;
            }

            await platformRepository.DeletePlatformAsync(existingPlatform.Id, token);
            return true;
        }

        public async Task<PlatformDTO> GetPlatformByIdAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var platform = await platformRepository.GetPlatformByIdAsync(id, token);
            Validation.ValidateNull(platform);

            return mapper.Map<PlatformDTO>(platform);
        }

        public async Task<IEnumerable<PlatformDTO>> GetAllPlatformsAsync(CancellationToken token)
        {
            var platforms = await platformRepository.GetAllPlatformsAsync(token);
            return platforms.Select(platform => mapper.Map<PlatformDTO>(platform));
        }

        public async Task<IEnumerable<GameDTO>> GetGameByPlatformAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var games = await platformRepository.GetGameByPlatformAsync(id, token);
            Validation.ValidateNull(games);

            return games.Select(game => mapper.Map<GameDTO>(game));
        }
    }
}