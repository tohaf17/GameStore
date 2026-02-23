using AutoMapper;
using GameStore.Application.DTO;
using GameStore.Repositories.Interfaces;
using GameStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Services
{
    public class GenreService:IGenreService
    {
        private readonly IGenreRepository genreRepository;
        private readonly IMapper mapper;

        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            this.genreRepository = genreRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GameDTO>> GetGameByGenreAsync(Guid id, CancellationToken token)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id is required");
            }
            var games = await genreRepository.GetGameByGenreAsync(id, token);
            if (games is null)
            {
                return null;
            }
            return games.Select(games => mapper.Map<GameDTO>(games));
        }
    }
}
