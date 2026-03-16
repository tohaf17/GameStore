using GameStore.Application.DTO;
using GameStore.Application.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace GameStoreTests.Controllers
{
    public class PlatformContollerTest : IClassFixture<GameStoreApiFactory>
    {
        private readonly HttpClient client;

        public PlatformContollerTest(GameStoreApiFactory factory)
        {
            client = factory.CreateClient();
        }


        [Fact]
        public async Task CreatePlatformAsync_ShouldReturnCreated_AndCorrectStatusCode()
        {
            var request = new CreatePlatformRequest
            {
                Platform = new CreatePlatformDto
                {
                    Type = "Mobile-" + Guid.NewGuid().ToString().Substring(0, 8)
                }
            };

            var response = await client.PostAsJsonAsync("/api/platforms", request);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }


        [Fact]
        public async Task UpdatePlatformAsync_ShouldReturnNoContent_WhenPlatformIsUpdated()
        {
            var createRequest = new CreatePlatformRequest { Platform = new CreatePlatformDto { Type = "Original Type" } };
            var createResponse = await client.PostAsJsonAsync("/api/platforms", createRequest);
            var createdPlatform = await createResponse.Content.ReadFromJsonAsync<PlatformDto>();

            var updateRequest = new UpdatePlatformRequest
            {
                Platform = new PlatformDto
                {
                    Id = createdPlatform!.Id,
                    Type = "Updated Type"
                }
            };

            var response = await client.PutAsJsonAsync("/api/platforms", updateRequest);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeletePlatformAsync_ShouldReturnNotFound_WhenPlatformDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await client.DeleteAsync($"/api/platforms/{nonExistentId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletePlatformAsync_ShouldReturnNoContent_WhenPlatformIsDeleted()
        {
            var createRequest = new CreatePlatformRequest { Platform = new CreatePlatformDto { Type = "To Delete" } };
            var createResponse = await client.PostAsJsonAsync("/api/platforms", createRequest);
            var createdPlatform = await createResponse.Content.ReadFromJsonAsync<PlatformDto>();

            var deleteResponse = await client.DeleteAsync($"/api/platforms/{createdPlatform!.Id}");

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
        [Fact]
        public async Task GetPlatformByIdAsync_ShouldReturnNotFound_WhenPlatformDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await client.GetAsync($"/api/platforms/{nonExistentId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPlatformByIdAsync_ShouldReturnOk_AndPlatform()
        {
            var createRequest = new CreatePlatformRequest { Platform = new CreatePlatformDto { Type = "Test Platform" } };
            var createResponse = await client.PostAsJsonAsync("/api/platforms", createRequest);
            var createdPlatform = await createResponse.Content.ReadFromJsonAsync<PlatformDto>();

            var getResponse = await client.GetAsync($"/api/platforms/{createdPlatform!.Id}");

            getResponse.EnsureSuccessStatusCode();
            var platform = await getResponse.Content.ReadFromJsonAsync<PlatformDto>();

            Assert.NotNull(platform);
            Assert.Equal(createdPlatform.Id, platform.Id);
        }



        [Fact]
        public async Task GetAllPlatformsAsync_ShouldReturnOk_AndListOfPlatforms()
        {
            var response = await client.GetAsync("/api/platforms");

            response.EnsureSuccessStatusCode();
            var platforms = await response.Content.ReadFromJsonAsync<List<PlatformDto>>();

            Assert.NotNull(platforms);
            Assert.IsType<List<PlatformDto>>(platforms);
        }


        [Fact]
        public async Task GetGameByPlatformAsync_ShouldReturnNotFound_WhenPlatformDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await client.GetAsync($"/api/platforms/{nonExistentId}/games");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGameByPlatformAsync_ShouldReturnOk_AndListOfGames()
        {
            var createRequest = new CreatePlatformRequest { Platform = new CreatePlatformDto { Type = "Platform for Games" } };
            var createResponse = await client.PostAsJsonAsync("/api/platforms", createRequest);
            var createdPlatform = await createResponse.Content.ReadFromJsonAsync<PlatformDto>();

            var getResponse = await client.GetAsync($"/api/platforms/{createdPlatform!.Id}/games");

            getResponse.EnsureSuccessStatusCode();
            var games = await getResponse.Content.ReadFromJsonAsync<List<GameDto>>();

            Assert.NotNull(games);
            Assert.IsType<List<GameDto>>(games);
        }
    }
}