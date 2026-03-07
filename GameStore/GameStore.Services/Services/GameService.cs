using AutoMapper;
using Azure.Core;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using GameStore.Domain.Entities;
using GameStore.Repositories.Interfaces;
using GameStore.Repositories.Repositories;
using GameStore.Services.Exceptions;
using GameStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork repository;

        private readonly IMapper mapper;

        public GameService(IUnitOfWork repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task<GameDto> CreateGameAsync(CreateGameRequest request, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            Validation.ValidateString(request.Game?.Name, nameof(request.Game.Name));

            return CreateGameInternalAsync(request, token);
        }

        private async Task<GameDto> CreateGameInternalAsync(CreateGameRequest request, CancellationToken token= default)
        {
            var game = mapper.Map<Game>(request.Game);
            if (string.IsNullOrWhiteSpace(game.Key))
            {
                game.Key = game.Name.ToLower().Replace(" ", "-");
            }

            foreach (var genreDto in request.Genres)
            {
                token.ThrowIfCancellationRequested();
                game.GameGenres.Add(new GameGenre { GameId = game.Id, GenreId = genreDto.Id });
            }

            foreach (var platformDto in request.Platforms)
            {
                token.ThrowIfCancellationRequested();
                game.GamePlatforms.Add(new GamePlatform { GameId = game.Id, PlatformId = platformDto.Id });
            }

            await repository.Games.AddGameAsync(game);
            await repository.SaveChangesAsync(token);

            return mapper.Map<GameDto>(game);
        }
        public async Task<IEnumerable<GenreDto>> GetGameGenresByKeyAsync(string key,CancellationToken token = default)
        {
            Validation.ValidateString(key, nameof(key));

            var genres = await repository.Games.GetGameGenresByKeyAsync(key, token);
            Validation.ValidateNull(genres);

            return mapper.Map<IEnumerable<GenreDto>>(genres);
        }
        public async Task<IEnumerable<PlatformDto>> GetGamePlatformsByKeyAsync(string key, CancellationToken token=default)
        {
            Validation.ValidateString(key, nameof(key));

            var platforms = await repository.Games.GetGamePlatformsByKeyAsync(key, token);
            Validation.ValidateNull(platforms);

            return mapper.Map<IEnumerable<PlatformDto>>(platforms);
        }
        public Task<bool> UpdateGameAsync(UpdateGameRequest request, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            Validation.ValidateGuid(request.Game.Id, nameof(request.Game.Id));

            return UpdateGameInternalAsync(request, token);
        }

        private async Task<bool> UpdateGameInternalAsync(UpdateGameRequest request, CancellationToken token = default)
        {
            var existingGame = await repository.Games.GetGameByIdAsync(request.Game.Id, token);
            Validation.ValidateNull(existingGame);

            mapper.Map(request.Game, existingGame);
            existingGame!.GameGenres.Clear();
            foreach (var genreDto in request.Genres)
            {
                token.ThrowIfCancellationRequested();
                existingGame.GameGenres.Add(new GameGenre { GameId = existingGame.Id, GenreId = genreDto.Id });
            }

            existingGame.GamePlatforms.Clear();
            foreach (var platformDto in request.Platforms)
            {
                token.ThrowIfCancellationRequested();
                existingGame.GamePlatforms.Add(new GamePlatform { GameId = existingGame.Id, PlatformId = platformDto.Id });
            }

            await repository.Games.UpdateGameAsync(existingGame);
            await repository.SaveChangesAsync(token);
            return true;
        }

        public async Task<bool> DeleteGameAsync(Guid id, CancellationToken token = default)
        {
            Validation.ValidateGuid(id, nameof(id));

            var existingGame = await repository.Games.GetGameByIdAsync(id, token);
            Validation.ValidateNull(existingGame);

            await repository.Games.DeleteGameAsync(existingGame!);
            await repository.SaveChangesAsync(token);
            return true;
        }
        public async Task<GameDto?> GetGameByKeyAsync(string key, CancellationToken token = default)
        {
            Validation.ValidateString(key, nameof(key));

            var game = await repository.Games.GetGameByKeyAsync(key, token);
            return game == null ? null : mapper.Map<GameDto>(game);
        }

        public async Task<GameDto> GetGameByIdAsync(Guid id, CancellationToken token = default)
        {
            Validation.ValidateGuid(id, nameof(id));

            var game = await repository.Games.GetGameByIdAsync(id, token);
            Validation.ValidateNull(game);

            return mapper.Map<GameDto>(game);
        }

        public async Task<bool> GetGameFilesAsync(Guid id, CancellationToken token = default)
        {
            Validation.ValidateGuid(id, nameof(id));

            var game = await repository.Games.GetGameByIdAsync(id, token);
            Validation.ValidateNull(game);

            await GenerateGameFile(game!,token);
            return true;
        }
        public async Task<IEnumerable<GameDto>> GetAllGamesAsync(CancellationToken token=default)
        {
            var games = await repository.Games.GetAllGamesAsync(token);
            return mapper.Map<IEnumerable<GameDto>>(games);
        }
        private async Task GenerateGameFile(Game? game,CancellationToken token = default)
        {
            string fileContent = $"Game ID: {game?.Id}" +
                $"\nName: {game?.Name}" +
                $"\nDescription: {game?.Description}" +
                $"\nGenres: {string.Join(", ", game?.GameGenres?.Select(g => g.Genre.Name) ?? Array.Empty<string>())}" +
                $"\nPlatforms: {string.Join(", ", game?.GamePlatforms?.Select(p => p.Platform.Type)??Array.Empty<string>())}";

            Directory.CreateDirectory("GameFiles");
            string filePath = Path.Combine("GameFiles", $"_{game?.Name}.txt");
            
            token.ThrowIfCancellationRequested();
            await File.WriteAllTextAsync(filePath, fileContent, token);
        }
    }
}
