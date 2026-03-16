using AutoMapper;
using GameStore.Application.DTO;
using GameStore.Domain.Entities;
using GameStore.Repositories.Interfaces;
using GameStore.Repositories.Repositories;
using GameStore.Services.Interfaces;
using System;
using GameStore.Application.Requests;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public PlatformService(IUnitOfWork repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task<PlatformDto> CreatePlatformAsync(CreatePlatformRequest request, CancellationToken token=default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return CreatePlatformInternalAsync(request, token);
        }

        public async Task<PlatformDto> CreatePlatformInternalAsync(CreatePlatformRequest request, CancellationToken token = default)
        {
            var platform = mapper.Map<Platform>(request.Platform);

            await repository.Platforms.AddPlatformAsync(platform);
            await repository.SaveChangesAsync(token);
            return mapper.Map<PlatformDto>(platform);
        }

        public Task<bool> UpdatePlatformAsync(UpdatePlatformRequest request, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return UpdatePlatformInternalAsync(request, token);
        }

        public async Task<bool> UpdatePlatformInternalAsync(UpdatePlatformRequest request, CancellationToken token = default)
        {
            var existingPlatform = await repository.Platforms.GetPlatformByIdAsync(request.Platform.Id, token);

            if (existingPlatform is null)
            {
                return false;
            }

            mapper.Map(request.Platform, existingPlatform);
            await repository.Platforms.UpdatePlatformAsync(existingPlatform);
            await repository.SaveChangesAsync(token);
            return true;
        }

        public async Task<bool> DeletePlatformAsync(Guid id, CancellationToken token = default)
        {

            var existingPlatform = await repository.Platforms.GetPlatformByIdAsync(id, token);

            if (existingPlatform is null)
            {
                return false;
            }

            await repository.Platforms.DeletePlatformAsync(existingPlatform);
            await repository.SaveChangesAsync(token);
            return true;
        }

        public async Task<PlatformDto> GetPlatformByIdAsync(Guid id, CancellationToken token = default)
        {

            var platform = await repository.Platforms.GetPlatformByIdAsync(id, token);
            

            return mapper.Map<PlatformDto>(platform);
        }

        public async Task<IEnumerable<PlatformDto>> GetAllPlatformsAsync(CancellationToken token = default)
        {
            var platforms = await repository.Platforms.GetAllPlatformsAsync(token);
            return platforms.Select(platform => mapper.Map<PlatformDto>(platform));
        }

        public async Task<IEnumerable<GameDto>> GetGameByPlatformAsync(Guid id, CancellationToken token = default)
        {
            var platform= await repository.Platforms.GetPlatformByIdAsync(id, token);
            if(platform == null)
            {
                return null;
            }
            var games = await repository.Platforms.GetGameByPlatformAsync(id, token);

            return games.Select(game => mapper.Map<GameDto>(game));
        }
    }
}