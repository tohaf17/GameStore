using AutoMapper;
using GameStore.Application.Requests;
using GameStore.Application.DTO;
using GameStore.Domain.Entities;
using GameStore.Repositories.Interfaces;
using GameStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Services.Services
{
    public class GameService:IGameService
    {
        private readonly IGameRepository gameRepository;

        private readonly IMapper mapper; 

        public GameService(IGameRepository gameRepository,IMapper mapper)
        {
            this.gameRepository = gameRepository;
            this.mapper = mapper;
        }
        public async Task<Guid> CreateGameAsync(CreateGameRequest request, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(request.Game.Name))
            {
                throw new ArgumentException("Game name is required");
            }

            var game = mapper.Map<Game>(request.Game);
            if (string.IsNullOrWhiteSpace(game.Key))
            {
                game.Key = game.Name.ToLower().Replace(" ", "-");
            }

            foreach (var genreDto in request.Genres)
            {
                game.GameGenres.Add(new GameGenre { GameId=game.Id,GenreId = genreDto.Id });
            }

            foreach (var platformDto in request.Platforms)
            {
                game.GamePlatforms.Add(new GamePlatform { GameId=game.Id,PlatformId = platformDto.Id });
            }

            await gameRepository.AddGameAsync(game, token);

            return game.Id;
        }

        public async Task<bool> UpdateGameAsync(UpdateGameRequest request, CancellationToken token)
        {
            if (request.Game.Id == Guid.Empty)
            {
                throw new ArgumentException("Game ID is required");
            }
            var existingGame = await gameRepository.GetGameByIdAsync(request.Game.Id, token);
            if (existingGame == null)
            {
                return false;
            }
            mapper.Map(request.Game, existingGame);
            existingGame.GameGenres.Clear();
            foreach (var genreDto in request.Genres)
            {
                existingGame.GameGenres.Add(new GameGenre { GameId=existingGame.Id,GenreId = genreDto.Id });
            }
            existingGame.GamePlatforms.Clear();
            foreach (var platformDto in request.Platforms)
            {
                existingGame.GamePlatforms.Add(new GamePlatform { GameId=existingGame.Id,PlatformId = platformDto.Id });
            }
            await gameRepository.AddGameAsync(existingGame, token);
            return true;
        }

        public async Task<bool> DeleteGameAsync(Guid id, CancellationToken token)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Game ID is required");
            }
            var existingGame = await gameRepository.GetGameByIdAsync(id, token);
            if (existingGame == null)
            {
                return false;
            }
            await gameRepository.DeleteGameAsync(existingGame, token);
            return true;
        }
        public async Task<GameDTO> GetGameByKeyAsync(string key,CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Game key is required");
            }
            var game = await gameRepository.GetGameByKeyAsync(key, token);
            if (game == null)
            {
                return null;
            }
            return mapper.Map<GameDTO>(game);
        }

        public async Task<GameDTO> GetGameByIdAsync(Guid id,CancellationToken token)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Game ID is required");
            }
            var game = await gameRepository.GetGameByIdAsync(id, token);
            if (game == null)
            {
                return null;
            }
            return mapper.Map<GameDTO>(game);
        }
    }
}
