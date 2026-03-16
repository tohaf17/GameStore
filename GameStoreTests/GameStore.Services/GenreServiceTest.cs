using AutoMapper;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using GameStore.Domain.Entities;
using GameStore.Repositories.Interfaces;
using GameStore.Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GameStoreTests.GameStore.Services
{
    public class GenreServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IGenreRepository> repositoryMock;
        private readonly GenreService service;

        public GenreServiceTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            repositoryMock = new Mock<IGenreRepository>();

            unitOfWorkMock.SetupGet(u => u.Genres).Returns(repositoryMock.Object);
            service = new GenreService(unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateGenreAsync(null!));
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldCallRepositoryAddGenreAsync_WhenRequestIsValid()
        {
            var request = new CreateGenreRequest
            {
                Genre = new CreateGenreDto { Name = "Action" }
            };

            var genreEntity = new Genre { Id = Guid.NewGuid(), Name = "Action" };
            var genreDto = new GenreDto { Id = genreEntity.Id, Name = "Action" };

            mapperMock.Setup(m => m.Map<Genre>(request.Genre)).Returns(genreEntity);
            mapperMock.Setup(m => m.Map<GenreDto>(genreEntity)).Returns(genreDto);

            var result = await service.CreateGenreAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Action", result.Name);

            repositoryMock.Verify(r => r.AddGenreAsync(genreEntity), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_ShouldReturnTrue_WhenGenreIsDeletedSuccessfully()
        {
            var genreId = Guid.NewGuid();
            var existingGenre = new Genre { Id = genreId, Name = "Action" };

            repositoryMock.Setup(r => r.GetGenreByIdAsync(genreId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingGenre);

            var result = await service.DeleteGenreAsync(genreId);

            Assert.True(result);
            repositoryMock.Verify(r => r.DeleteGenreAsync(existingGenre), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateGenreAsync(null!));
        }

        [Fact]
        public async Task UpdateGenreAsync_ShouldReturnTrue_WhenGenreIsUpdatedSuccessfully()
        {
            var request = new UpdateGenreRequest
            {
                Genre = new GenreDto { Id = Guid.NewGuid(), Name = "Updated Action" }
            };

            var existingGenre = new Genre { Id = request.Genre.Id, Name = "Action" };

            repositoryMock.Setup(r => r.GetGenreByIdAsync(request.Genre.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingGenre);

            var result = await service.UpdateGenreAsync(request);

            Assert.True(result);
            mapperMock.Verify(m => m.Map(request.Genre, existingGenre), Times.Once);
            repositoryMock.Verify(r => r.UpdateGenreAsync(existingGenre), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnMappedGenreDto_WhenGenreExists()
        {
            var genreId = Guid.NewGuid();
            var genreEntity = new Genre { Id = genreId, Name = "RPG" };
            var genreDto = new GenreDto { Id = genreId, Name = "RPG" };

            repositoryMock.Setup(r => r.GetGenreByIdAsync(genreId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(genreEntity);
            mapperMock.Setup(m => m.Map<GenreDto>(genreEntity)).Returns(genreDto);

            var result = await service.GetGenreByIdAsync(genreId);

            Assert.NotNull(result);
            Assert.Equal(genreId, result.Id);
            Assert.Equal("RPG", result.Name);
        }

        [Fact]
        public async Task GetAllGenresAsync_ShouldReturnAllGenres()
        {
            var genres = new List<Genre>
            {
                new Genre { Id = Guid.NewGuid(), Name = "Action" },
                new Genre { Id = Guid.NewGuid(), Name = "RPG" }
            };

            repositoryMock.Setup(r => r.GetAllGenresAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(genres);
            mapperMock.Setup(m => m.Map<GenreDto>(It.IsAny<Genre>()))
                      .Returns((Genre g) => new GenreDto { Id = g.Id, Name = g.Name });

            var result = await service.GetAllGenresAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGenresByParentIdAsync_ShouldReturnMappedGenres()
        {
            var parentId = Guid.NewGuid();
            var genres = new List<Genre>
            {
                new Genre { Id = Guid.NewGuid(), Name = "Sub-genre 1", ParentGenreId = parentId }
            };

            repositoryMock.Setup(r => r.GetGenreByIdAsync(parentId, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new Genre { Id = parentId, Name = "Test Genre" });
            repositoryMock.Setup(r => r.GetGenresByParentIdAsync(parentId, It.IsAny<CancellationToken>()))
          .ReturnsAsync(genres);
            mapperMock.Setup(m => m.Map<GenreDto>(It.IsAny<Genre>()))
                      .Returns((Genre g) => new GenreDto { Id = g.Id, Name = g.Name });

            var result = await service.GetGenresByParentIdAsync(parentId);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetGameByGenreAsync_ShouldReturnMappedGames()
        {
            var genreId = Guid.NewGuid();
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "game-1" }
            };

            repositoryMock.Setup(r => r.GetGenreByIdAsync(genreId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new Genre { Id = genreId, Name = "Action" });
            repositoryMock.Setup(r => r.GetGameByGenreAsync(genreId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(games);

            mapperMock.Setup(m => m.Map<GameDto>(It.IsAny<Game>()))
                      .Returns((Game g) => new GameDto { Id = g.Id, Name = g.Name });

            var result = await service.GetGameByGenreAsync(genreId);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldThrowOperationCanceledException_WhenCancellationIsRequested()
        {
            var request = new CreateGenreRequest { Genre = new CreateGenreDto { Name = "Action" } };

            using var cancellationTokenSource = new CancellationTokenSource();
            await cancellationTokenSource.CancelAsync();

            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ThrowsAsync(new OperationCanceledException());

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => service.CreateGenreAsync(request, cancellationTokenSource.Token));
        }
    }
}