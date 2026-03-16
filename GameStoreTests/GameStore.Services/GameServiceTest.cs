using AutoMapper;
using GameStore.Application;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using GameStore.Domain.Entities;
using GameStore.Repositories.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStoreTests.GameStore.Services
{
    public class GameServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IGameRepository> repositoryMock;
        private readonly GameService service;

        public GameServiceTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            repositoryMock = new Mock<IGameRepository>();
            unitOfWorkMock.SetupGet(u => u.Games).Returns(repositoryMock.Object);
            service = new GameService(unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateGameAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateGameAsync(null!));

        }
        [Fact]
        public async Task CreateGameAsync_ShouldCallRepositoryAddGameAsync_WhenRequestIsValid()
        {

            var request = new CreateGameRequest
            {
                Game = new CreateGameDto { Name = "New Game", Key = "" },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };

            var gameEntity = new Game { Id = Guid.NewGuid(), Name = "New Game", Key = "" };
            var gameDto = new GameDto { Id = gameEntity.Id, Name = "New Game", Key = "new-game" };

            mapperMock.Setup(m => m.Map<Game>(request.Game)).Returns(gameEntity);
            mapperMock.Setup(m => m.Map<GameDto>(gameEntity)).Returns(gameDto);

            var result = await service.CreateGameAsync(request);

            Assert.NotNull(result);
            Assert.Equal("new-game", gameEntity.Key);
            Assert.Single(gameEntity.GameGenres);
            Assert.Single(gameEntity.GamePlatforms);

            repositoryMock.Verify(r => r.AddGameAsync(It.Is<Game>(g => g.Id == gameEntity.Id && g.Name == gameEntity.Name && g.Key == gameEntity.Key)), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task ShouldReturnMappedGenresWhenGettingGenresByKey()
        {
            var key = "test-game";
            var genres = new List<Genre> { new Genre { Id = Guid.NewGuid(), Name = "Action" } };
            var genreDtos = new List<GenreDto> { new GenreDto { Id = genres[0].Id, Name = "Action" } };
            repositoryMock.Setup(r => r.GetGameByKeyAsync(key, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Game { Key = key });
            repositoryMock.Setup(r => r.GetGameGenresByKeyAsync(key, It.IsAny<CancellationToken>())).ReturnsAsync(genres);
            mapperMock.Setup(m => m.Map<IEnumerable<GenreDto>>(genres)).Returns(genreDtos);

            var result = await service.GetGameGenresByKeyAsync(key);

            Assert.NotNull(result);
            Assert.Single(result);
            repositoryMock.Verify(r => r.GetGameGenresByKeyAsync(key, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public void ShouldReturnMappedPlatformsWhenGettingPlatformsByKey()
        {
            var key = "test-game";
            var platforms = new List<Platform> { new Platform { Id = Guid.NewGuid(), Type = "PC" } };
            var platformDtos = new List<PlatformDto> { new PlatformDto { Id = platforms[0].Id, Type = "PC" } };
            repositoryMock.Setup(r => r.GetGameByKeyAsync(key, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Game { Key = key });
            repositoryMock.Setup(r => r.GetGamePlatformsByKeyAsync(key, It.IsAny<CancellationToken>())).ReturnsAsync(platforms);
            mapperMock.Setup(m => m.Map<IEnumerable<PlatformDto>>(platforms)).Returns(platformDtos);

            var result = service.GetGamePlatformsByKeyAsync(key)?.Result;

            Assert.NotNull(result);
            Assert.Single(result);

            repositoryMock.Verify(r => r.GetGamePlatformsByKeyAsync(key, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateGameAsync_ShouldThrowOperationCanceledException_WhenCancellationIsRequested()
        {
            var request = new CreateGameRequest
            {
                Game = new CreateGameDto { Name = "New Game", Key = "" },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var gameEntity = new Game { Id = Guid.NewGuid(), Name = "New Game", Key = "" };
            mapperMock.Setup(m => m.Map<Game>(request.Game)).Returns(gameEntity);
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                await cancellationTokenSource.CancelAsync();
                await Assert.ThrowsAsync<OperationCanceledException>(() => service.CreateGameAsync(request, cancellationTokenSource.Token));
            }

        }
        [Fact]
        public async Task GetGameGenresByKeyAsync_ShouldThrowOperationCanceledException_WhenCancellationIsRequested()
        {
            var key = "test-game";
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                await cancellationTokenSource.CancelAsync();
                repositoryMock.Setup(r => r.GetGameByKeyAsync(key, It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new OperationCanceledException());
                repositoryMock.Setup(r => r.GetGameGenresByKeyAsync(key, It.IsAny<CancellationToken>())).ThrowsAsync(new OperationCanceledException());
                await Assert.ThrowsAsync<OperationCanceledException>(() => service.GetGameGenresByKeyAsync(key, cancellationTokenSource.Token));
            }
        }
        [Fact]
        public async Task GetGamePlatformsByKeyAsync_ShouldThrowOperationCanceledException_WhenCancellationIsRequested()
        {
            var key = "test-game";
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                await cancellationTokenSource.CancelAsync();
                repositoryMock.Setup(r => r.GetGameByKeyAsync(key, It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new OperationCanceledException());
                repositoryMock.Setup(r => r.GetGamePlatformsByKeyAsync(key, It.IsAny<CancellationToken>())).ThrowsAsync(new OperationCanceledException());
                await Assert.ThrowsAsync<OperationCanceledException>(() => service.GetGamePlatformsByKeyAsync(key, cancellationTokenSource.Token));
            }
        }
        [Fact]
        public async Task CreateGameAsync_ShouldHandleEmptyKeyByGeneratingFromName()
        {
            var request = new CreateGameRequest
            {
                Game = new CreateGameDto { Name = "Test Game", Key = "" },
                Genres = new List<GenreDto>(),
                Platforms = new List<PlatformDto>()
            };
            var gameEntity = new Game { Id = Guid.NewGuid(), Name = "Test Game", Key = "" };
            var gameDto = new GameDto { Id = gameEntity.Id, Name = "Test Game", Key = "test-game" };
            mapperMock.Setup(m => m.Map<Game>(request.Game)).Returns(gameEntity);
            mapperMock.Setup(m => m.Map<GameDto>(gameEntity)).Returns(gameDto);
            var result = await service.CreateGameAsync(request);
            Assert.NotNull(result);
            Assert.Equal("test-game", gameEntity.Key);
        }
        [Fact]
        public async Task UpdateGameAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateGameAsync(null!));
        }

        [Fact]
        public async Task UpdateGameAsync_ShouldReturnFalse_WhenGameDoesNotExist()
        {
            var request = new UpdateGameRequest
            {
                Game = new GameDto { Id = Guid.NewGuid(), Name = "Nonexistent Game", Key = "nonexistent-game" },
                Genres = new List<GenreDto>(),
                Platforms = new List<PlatformDto>()
            };

            repositoryMock.Setup(r => r.GetGameByIdAsync(request.Game.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Game?)null);

            var result = await service.UpdateGameAsync(request);

            Assert.False(result);
            repositoryMock.Verify(r => r.UpdateGameAsync(It.IsAny<Game>()), Times.Never);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
        [Fact]
        public void UpdateGameAsync_ShouldReturnTrue_WhenGameIsUpdatedSuccessfully()
        {
            var request = new UpdateGameRequest
            {
                Game = new GameDto { Id = Guid.NewGuid(), Name = "Existing Game", Key = "existing-game" },
                Genres = new List<GenreDto>(),
                Platforms = new List<PlatformDto>()
            };
            var existingGame = new Game { Id = request.Game.Id, Name = "Existing Game", Key = "existing-game" };
            repositoryMock.Setup(r => r.GetGameByIdAsync(request.Game.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingGame);
            var result = service.UpdateGameAsync(request)?.Result;
            Assert.True(result);
            repositoryMock.Verify(r => r.UpdateGameAsync(It.Is<Game>(g => g.Id == existingGame.Id && g.Name == request.Game.Name && g.Key == request.Game.Key)), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateGameAsync_ShouldThrowOperationCanceledException_WhenCancellationIsRequested()
        {
            var gameId = Guid.NewGuid();
            var request = new UpdateGameRequest
            {
                Game = new GameDto { Id = gameId, Name = "Existing Game", Key = "existing-game" },
                
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto>()
            };

            var existingGame = new Game
            {
                Id = gameId,
                Name = "Existing Game",
                Key = "existing-game",
                GameGenres = new List<GameGenre>(),
                GamePlatforms = new List<GamePlatform>()
            };

            repositoryMock.Setup(r => r.GetGameByIdAsync(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(existingGame);

            using var cancellationTokenSource = new CancellationTokenSource();
            await cancellationTokenSource.CancelAsync();

            await Assert.ThrowsAsync<OperationCanceledException>(() => service.UpdateGameAsync(request, cancellationTokenSource.Token));
        }
        [Fact]
        public void DeleteGameAsync_ShouldReturnFalse_WhenGameDoesNotExist()
        {
            var gameId = Guid.NewGuid();
            repositoryMock.Setup(r => r.GetGameByIdAsync(gameId, It.IsAny<CancellationToken>())).ReturnsAsync((Game)null!);
            var result = service.DeleteGameAsync(gameId)?.Result;
            Assert.False(result);
        }
        [Fact]
        public async Task ShouldReturnTrueWhenGameIsDeletedSuccessfully()
        {
            var gameId = Guid.NewGuid();
            var existingGame = new Game { Id = gameId, Name = "Existing Game", Key = "existing-game" };

            repositoryMock.Setup(r => r.GetGameByIdAsync(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(existingGame);

            var result = await service.DeleteGameAsync(gameId);

            Assert.True(result);
            repositoryMock.Verify(r => r.DeleteGameAsync(existingGame), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public void GetGameByKeyAsync_ShouldReturnNull_WhenGameDoesNotExist()
        {
            var key = "nonexistent-game";
            repositoryMock.Setup(r => r.GetGameByKeyAsync(key, It.IsAny<CancellationToken>())).ReturnsAsync((Game)null!);
            var result = service.GetGameByKeyAsync(key)?.Result;
            Assert.Null(result);
        }
        [Fact]
        public async Task GetGameByKeyAsync_ShouldReturnMappedGameDto_WhenGameExists()
        {
            var key = "existing";
            var gameEntity = new Game { Id = Guid.NewGuid(), Name = "Existing Game", Key = key };
            var gameDto = new GameDto { Id = gameEntity.Id, Name = "Existing Game", Key = key };

            repositoryMock.Setup(r => r.GetGameByKeyAsync(key, It.IsAny<CancellationToken>())).ReturnsAsync(gameEntity);
            mapperMock.Setup(m => m.Map<GameDto>(gameEntity)).Returns(gameDto);

            var result = await service.GetGameByKeyAsync(key);

            Assert.NotNull(result);
            Assert.Equal(key, result.Key);
        }
        [Fact]
        public void GetGameByIdAsync_ShouldReturnNull_WhenGameDoesNotExist()
        {
            var gameId = Guid.NewGuid();
            repositoryMock.Setup(r => r.GetGameByIdAsync(gameId, It.IsAny<CancellationToken>())).ReturnsAsync((Game)null!);
            var result = service.GetGameByIdAsync(gameId)?.Result;
            Assert.Null(result);
        }
        [Fact]
        public void GetGameFilesAsync_ShouldReturnFiles_WhenGameExists()
        {
            var gameId = Guid.NewGuid();
            repositoryMock.Setup(r => r.GetGameByIdAsync(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(new Game { Id = gameId, Name = "Existing Game", Key = "existing-game" });
            var result = service.GetGameFilesAsync(gameId)?.Result;
            Assert.NotNull(result);
        }
        [Fact]
        public void GetAllGamesAsync_ShouldReturnAllGames()
        {
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "game-1" },
                new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "game-2" }
            };
            var gameDtos = new List<GameDto>
            {
                new GameDto { Id = games[0].Id, Name = "Game 1", Key = "game-1" },
                new GameDto { Id = games[1].Id, Name = "Game 2", Key = "game-2" }
            };
            repositoryMock.Setup(r => r.GetAllGamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(games);
            mapperMock.Setup(m => m.Map<IEnumerable<GameDto>>(games)).Returns(gameDtos);
            var result = service.GetAllGamesAsync()?.Result;
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
       
    }
}