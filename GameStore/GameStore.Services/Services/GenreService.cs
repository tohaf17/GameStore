using AutoMapper;
using GameStore.Application.DTO;
using GameStore.Repositories.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.Requests;

namespace GameStore.Services.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public GenreService(IUnitOfWork repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task<GenreDto> CreateGenreAsync(CreateGenreRequest request, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            return CreateGenreInternalAsync(request, token);
        }

        private async Task<GenreDto> CreateGenreInternalAsync(CreateGenreRequest request, CancellationToken token = default)
        {
            var genre = mapper.Map<Genre>(request.Genre);

            await repository.Genres.AddGenreAsync(genre);
            await repository.SaveChangesAsync(token);
            return mapper.Map<GenreDto>(genre);
        }

        public async Task<bool> DeleteGenreAsync(Guid id, CancellationToken token = default)
        {
            var existingGenre = await repository.Genres.GetGenreByIdAsync(id, token);
            if (existingGenre == null)
            {
                return false;
            }
            await repository.Genres.DeleteGenreAsync(existingGenre);
            
            await repository.SaveChangesAsync(token);
            return true;
        }

        public Task<bool> UpdateGenreAsync(UpdateGenreRequest request, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            return UpdateGenreInternalAsync(request, token);
        }

        private async Task<bool> UpdateGenreInternalAsync(UpdateGenreRequest request, CancellationToken token = default)
        {
            var existingGenre = await repository.Genres.GetGenreByIdAsync(request.Genre.Id, token);
            mapper.Map(request.Genre, existingGenre);

            await repository.Genres.UpdateGenreAsync(existingGenre!);
            
                await repository.SaveChangesAsync(token);
                return true;
            
        }

        public async Task<IEnumerable<GenreDto>?> GetGenresByParentIdAsync(Guid id, CancellationToken token = default)
        {
            var genre = await repository.Genres.GetGenreByIdAsync(id, token);

            if (genre == null)
            {
                return null;
            }

            var genres = await repository.Genres.GetGenresByParentIdAsync(id, token);


            return genres.Select(genre => mapper.Map<GenreDto>(genre));
        }

        public async Task<GenreDto> GetGenreByIdAsync(Guid id, CancellationToken token = default)
        {

            var genre = await repository.Genres.GetGenreByIdAsync(id, token);

            return mapper.Map<GenreDto>(genre);
        }

        public async Task<IEnumerable<GenreDto>?> GetAllGenresAsync(CancellationToken token=default)
        {
            var genres = await repository.Genres.GetAllGenresAsync(token);

            return genres.Select(genre => mapper.Map<GenreDto>(genre));
        }

        public async Task<IEnumerable<GameDto>?> GetGameByGenreAsync(Guid id, CancellationToken token = default)
        {
            var genre = await repository.Genres.GetGenreByIdAsync(id, token);

            if (genre == null)
            {
                return null;
            }
            var games = await repository.Genres.GetGameByGenreAsync(id, token);

            return games.Select(game => mapper.Map<GameDto>(game));
        }
    }
}