using AutoMapper;
using GameStore.Services.Interfaces;
using System;
using GameStore.Application.DTO;
using System.Collections.Generic;
using GameStore.Repositories.Interfaces;
using System.Text;
using GameStore.Services.Exceptions;

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
