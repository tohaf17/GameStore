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
        public async Task<IEnumerable<GameDTO>> GetGameByPlatformAsync(Guid id,CancellationToken token)
        {
            if (id==null)
            {
                throw new ArgumentNullException("Id is required");
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
