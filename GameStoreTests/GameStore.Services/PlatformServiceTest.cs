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
    public class PlatformServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPlatformRepository> repositoryMock;
        private readonly PlatformService service;

        public PlatformServiceTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            repositoryMock = new Mock<IPlatformRepository>();

            unitOfWorkMock.SetupGet(u => u.Platforms).Returns(repositoryMock.Object);
            service = new PlatformService(unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreatePlatformAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreatePlatformAsync(null!));
        }

        [Fact]
        public async Task CreatePlatformAsync_ShouldCallRepositoryAddPlatformAsync_WhenRequestIsValid()
        {
            var request = new CreatePlatformRequest
            {
                Platform = new CreatePlatformDto { Type = "PC" }
            };

            var platformEntity = new Platform { Id = Guid.NewGuid(), Type = "PC" };
            var platformDto = new PlatformDto { Id = platformEntity.Id, Type = "PC" };

            mapperMock.Setup(m => m.Map<Platform>(request.Platform)).Returns(platformEntity);
            mapperMock.Setup(m => m.Map<PlatformDto>(platformEntity)).Returns(platformDto);

            var result = await service.CreatePlatformAsync(request);

            Assert.NotNull(result);
            Assert.Equal("PC", result.Type);

            repositoryMock.Verify(r => r.AddPlatformAsync(platformEntity), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePlatformAsync_ShouldThrowOperationCanceledException_WhenCancellationIsRequested()
        {
            var request = new CreatePlatformRequest { Platform = new CreatePlatformDto { Type = "PC" } };

            using var cancellationTokenSource = new CancellationTokenSource();
            await cancellationTokenSource.CancelAsync();

            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ThrowsAsync(new OperationCanceledException());

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => service.CreatePlatformAsync(request, cancellationTokenSource.Token));
        }

        [Fact]
        public async Task UpdatePlatformAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdatePlatformAsync(null!));
        }

        [Fact]
        public async Task UpdatePlatformAsync_ShouldReturnFalse_WhenPlatformDoesNotExist()
        {
            var request = new UpdatePlatformRequest
            {
                Platform = new PlatformDto { Id = Guid.NewGuid(), Type = "Non-existent Platform" }
            };

            repositoryMock.Setup(r => r.GetPlatformByIdAsync(request.Platform.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Platform?)null);

            var result = await service.UpdatePlatformAsync(request);

            Assert.False(result);
            repositoryMock.Verify(r => r.UpdatePlatformAsync(It.IsAny<Platform>()), Times.Never);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePlatformAsync_ShouldReturnTrue_WhenPlatformIsUpdatedSuccessfully()
        {
            var request = new UpdatePlatformRequest
            {
                Platform = new PlatformDto { Id = Guid.NewGuid(), Type = "Updated Xbox" }
            };

            var existingPlatform = new Platform { Id = request.Platform.Id, Type = "Xbox" };

            repositoryMock.Setup(r => r.GetPlatformByIdAsync(request.Platform.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingPlatform);

            var result = await service.UpdatePlatformAsync(request);

            Assert.True(result);
            mapperMock.Verify(m => m.Map(request.Platform, existingPlatform), Times.Once);
            repositoryMock.Verify(r => r.UpdatePlatformAsync(existingPlatform), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletePlatformAsync_ShouldReturnFalse_WhenPlatformDoesNotExist()
        {
            var platformId = Guid.NewGuid();

            repositoryMock.Setup(r => r.GetPlatformByIdAsync(platformId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Platform?)null);

            var result = await service.DeletePlatformAsync(platformId);

            Assert.False(result);
            repositoryMock.Verify(r => r.DeletePlatformAsync(It.IsAny<Platform>()), Times.Never);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeletePlatformAsync_ShouldReturnTrue_WhenPlatformIsDeletedSuccessfully()
        {
            var platformId = Guid.NewGuid();
            var existingPlatform = new Platform { Id = platformId, Type = "PlayStation" };

            repositoryMock.Setup(r => r.GetPlatformByIdAsync(platformId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingPlatform);

            var result = await service.DeletePlatformAsync(platformId);

            Assert.True(result);
            repositoryMock.Verify(r => r.DeletePlatformAsync(existingPlatform), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetPlatformByIdAsync_ShouldReturnMappedPlatformDto_WhenPlatformExists()
        {
            var platformId = Guid.NewGuid();
            var platformEntity = new Platform { Id = platformId, Type = "PC" };
            var platformDto = new PlatformDto { Id = platformId, Type = "PC" };

            repositoryMock.Setup(r => r.GetPlatformByIdAsync(platformId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(platformEntity);
            mapperMock.Setup(m => m.Map<PlatformDto>(platformEntity)).Returns(platformDto);

            var result = await service.GetPlatformByIdAsync(platformId);

            Assert.NotNull(result);
            Assert.Equal(platformId, result.Id);
            Assert.Equal("PC", result.Type);
        }

        [Fact]
        public async Task GetAllPlatformsAsync_ShouldReturnAllPlatforms()
        {
            var platforms = new List<Platform>
            {
                new Platform { Id = Guid.NewGuid(), Type = "PC" },
                new Platform { Id = Guid.NewGuid(), Type = "PlayStation" }
            };

            repositoryMock.Setup(r => r.GetAllPlatformsAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(platforms);

            mapperMock.Setup(m => m.Map<PlatformDto>(It.IsAny<Platform>()))
                      .Returns((Platform p) => new PlatformDto { Id = p.Id, Type = p.Type });

            var result = await service.GetAllPlatformsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGameByPlatformAsync_ShouldReturnMappedGames()
        {
            var platformId = Guid.NewGuid();
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "game-1" }
            };

            repositoryMock.Setup(r => r.GetGameByPlatformAsync(platformId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(games);

            mapperMock.Setup(m => m.Map<GameDto>(It.IsAny<Game>()))
                      .Returns((Game g) => new GameDto { Id = g.Id, Name = g.Name });

            var result = await service.GetGameByPlatformAsync(platformId);

            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}