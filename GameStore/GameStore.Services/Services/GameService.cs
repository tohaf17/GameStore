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
        private readonly IGameRepository gameRepository;

        private readonly IMapper mapper;

        public GameService(IGameRepository gameRepository, IMapper mapper)
        {
            this.gameRepository = gameRepository;
            this.mapper = mapper;
        }
        
        public async Task<Guid> CreateGameAsync(CreateGameRequest request, CancellationToken token)
        {

            ArgumentNullException.ThrowIfNull(request);
            Validation.ValidateString(request.Game?.Name, nameof(request.Game.Name));

            var game = mapper.Map<Game>(request.Game);
            if (string.IsNullOrWhiteSpace(game.Key))
            {
                game.Key = game.Name.ToLower().Replace(" ", "-");
            }

            foreach (var genreDto in request.Genres)
            {
                game.GameGenres.Add(new GameGenre { GameId = game.Id, GenreId = genreDto.Id });
            }
            
            foreach (var platformDto in request.Platforms)
            {
                game.GamePlatforms.Add(new GamePlatform { GameId = game.Id, PlatformId = platformDto.Id });
            }

            await gameRepository.AddGameAsync(game, token);

            return game.Id;
        }
        public async Task<IEnumerable<GenreDTO>> GetGameGenresByKeyAsync(string key,CancellationToken token)
        {
            Validation.ValidateString(key, nameof(key));

            var genres = await gameRepository.GetGameGenresByKeyAsync(key, token);
            Validation.ValidateNull(genres);

            return mapper.Map<IEnumerable<GenreDTO>>(genres);
        }
        public async Task<IEnumerable<PlatformDTO>> GetGamePlatformsByKeyAsync(string key, CancellationToken token)
        {
            Validation.ValidateString(key, nameof(key));

            var platforms = await gameRepository.GetGamePlatformsByKeyAsync(key, token);
            Validation.ValidateNull(platforms);

            return mapper.Map<IEnumerable<PlatformDTO>>(platforms);
        }
        public async Task<bool> UpdateGameAsync(UpdateGameRequest request, CancellationToken token)
        {

            ArgumentNullException.ThrowIfNull(request);
            Validation.ValidateGuid(request.Game.Id, nameof(request.Game.Id));

            var existingGame = await gameRepository.GetGameByIdAsync(request.Game.Id, token);
            Validation.ValidateNull(existingGame);

            mapper.Map(request.Game, existingGame);

            existingGame!.GameGenres.Clear();
            foreach (var genreDto in request.Genres)
            {
                existingGame.GameGenres.Add(new GameGenre { GameId = existingGame.Id, GenreId = genreDto.Id });
            }
            existingGame.GamePlatforms.Clear();
            foreach (var platformDto in request.Platforms)
            {
                existingGame.GamePlatforms.Add(new GamePlatform { GameId = existingGame.Id, PlatformId = platformDto.Id });
            }
            await gameRepository.AddGameAsync(existingGame, token);
            return true;
        }

        public async Task<bool> DeleteGameAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var existingGame = await gameRepository.GetGameByIdAsync(id, token);
            Validation.ValidateNull(existingGame);

            await gameRepository.DeleteGameAsync(existingGame!, token);
            return true;
        }
        public async Task<GameDTO?> GetGameByKeyAsync(string key, CancellationToken token)
        {
            Validation.ValidateString(key, nameof(key));

            var game = await gameRepository.GetGameByKeyAsync(key, token);
            return game == null ? null : mapper.Map<GameDTO>(game);
        }

        public async Task<GameDTO> GetGameByIdAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var game = await gameRepository.GetGameByIdAsync(id, token);
            Validation.ValidateNull(game);

            return mapper.Map<GameDTO>(game);
        }

        public async Task<bool> GetGameFilesAsync(Guid id, CancellationToken token)
        {
            Validation.ValidateGuid(id, nameof(id));

            var game = await gameRepository.GetGameByIdAsync(id, token);
            Validation.ValidateNull(game);

            GenerateGameFile(game!);
            return true;
        }
        public async Task<IEnumerable<GameDTO>> GetAllGamesAsync(CancellationToken token)
        {
            var games = await gameRepository.GetAllGamesAsync(token);
            return mapper.Map<IEnumerable<GameDTO>>(games);
        }
        private static void GenerateGameFile(Game? game)
        {
            string fileContent = $"Game ID: {game?.Id}\nName: {game?.Name}\nDescription: {game?.Description}\nGenres: {string.Join(", ", game?.GameGenres?.Select(g => g.Genre.Name) ?? Array.Empty<string>())}\nPlatforms: {string.Join(", ", game?.GamePlatforms?.Select(p => p.Platform.Type)??Array.Empty<string>())}";
            Directory.CreateDirectory("GameFiles");
            string filePath = Path.Combine("GameFiles", $"_{game?.Name}.txt");
            using (StreamWriter writer=new StreamWriter(filePath))
            {
                writer.Write(fileContent);
            }
        }
    }
}
