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
    public class GenreService : IGenreService
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
            ArgumentNullException.ThrowIfNull(request);

            var genre = mapper.Map<Genre>(request.Genre);

            await genreRepository.AddGenreAsync(genre, token);

            return genre.Id;
        }

        public async Task<bool> DeleteGenreAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var existingGenre = await genreRepository.GetGenreByIdAsync(id, token);
            Validation.ValidateNull(existingGenre);

            if (await genreRepository.DeleteGenreAsync(id, token))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateGenreAsync(UpdateGenreRequest request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request);
            Validation.ValidateGuid(request.Genre.Id, nameof(request.Genre.Id));

            var existingGenre = await genreRepository.GetGenreByIdAsync(request.Genre.Id, token);
            Validation.ValidateNull(existingGenre);

            mapper.Map(request.Genre, existingGenre);
            if (await genreRepository.UpdateGenreAsync(existingGenre!, token))
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<GenreDTO>> GetGenresByParentIdAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var genres = await genreRepository.GetGenresByParentIdAsync(id, token);
            Validation.ValidateNull(genres);

            return genres.Select(genre => mapper.Map<GenreDTO>(genre));
        }

        public async Task<GenreDTO> GetGenreByIdAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var genre = await genreRepository.GetGenreByIdAsync(id, token);
            Validation.ValidateNull(genre);

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
            Validation.ValidateGuid(id, nameof(id));

            var games = await genreRepository.GetGameByGenreAsync(id, token);
            Validation.ValidateNull(games);

            return games.Select(game => mapper.Map<GameDTO>(game));
        }
    }
}