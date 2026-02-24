using AutoMapper;
using GameStore.Application.DTO;
using GameStore.Repositories.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Domain.Entities;
using GameStore.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.Requests;

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

        public async Task<Guid> CreateGenreAsync(CreateGenreRequest request, CancellationToken token)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var genre = mapper.Map<Genre>(request.Genre);

            await genreRepository.AddGenreAsync(genre, token);

            return genre.Id;
        }
        
        public async Task<IEnumerable<GenreDTO>> GetGenresByParentIdAsync(Guid id,CancellationToken token)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id is required");
            }
            var genres = await genreRepository.GetGenresByParentIdAsync(id, token);
            if(genres is null)
            {
                throw new NotFoundException($"No genres found for parent genre with id {id}");
            }
            return genres.Select(genre => mapper.Map<GenreDTO>(genre));
        }
       public async Task<GenreDTO> GetGenreByIdAsync(Guid id,CancellationToken token)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id is required");
            }
            var genre = await genreRepository.GetGenreByIdAsync(id, token);
            if(genre is null)
            {
                               throw new NotFoundException($"Genre with id {id} not found");

            }
            return mapper.Map<GenreDTO>(genre);
        }
        public async Task<IEnumerable<GenreDTO>> GetAllGenresAsync(CancellationToken token)
        {
            var genres = await genreRepository.GetAllGenresAsync(token);
            if (genres is null || !genres.Any())
            {
                throw new NotFoundException("No genres found");
            }
            return genres.Select(genre => mapper.Map<GenreDTO>(genre));
        }
        public async Task<IEnumerable<GameDTO>> GetGameByGenreAsync(Guid id, CancellationToken token)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id is required");
            }
            var games = await genreRepository.GetGameByGenreAsync(id, token);
            if (games is null)
            {
                throw new NotFoundException($"No games found for genre with id {id}");
            }
            return games.Select(games => mapper.Map<GameDTO>(games));
        }
    }
}
