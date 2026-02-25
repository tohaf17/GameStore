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
    public class PlatformService:IPlatformService
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
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var genre = mapper.Map<Platform>(request.Platform);

            await platformRepository.AddPlatformAsync(genre, token);

            return genre.Id;
        }
        public async Task<bool> UpdatePlatformAsync(UpdatePlatformRequest request, CancellationToken token)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
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
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"Id {id} is required");
            }
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
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"Id {id} is required");
            }
            var platform = await platformRepository.GetPlatformByIdAsync(id, token);
            if (platform is null)
            {
                throw new NotFoundException("Platform not found for the given id");
            }
            return mapper.Map<PlatformDTO>(platform);
        }
        public async Task<IEnumerable<PlatformDTO>> GetAllPlatformsAsync(CancellationToken token)
        {
            var platforms = await platformRepository.GetAllPlatformsAsync(token);
            return platforms.Select(platform => mapper.Map<PlatformDTO>(platform));
        }
        public async Task<IEnumerable<GameDTO>> GetGameByPlatformAsync(Guid id,CancellationToken token)
        {
            if (id==Guid.Empty)
            {
                throw new ArgumentNullException($"Id {id} is required");
            }
            var games = await platformRepository.GetGameByPlatformAsync(id, token);
            if(games is null)
            {
                throw new NotFoundException("Games not found for the given platform id");
            }
            return games.Select(games => mapper.Map<GameDTO>(games));
        }  
    }
}
