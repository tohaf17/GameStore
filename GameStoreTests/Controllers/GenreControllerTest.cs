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
    public class GenreControllerTest : IClassFixture<GameStoreApiFactory>
    {
        private readonly HttpClient client;

        public GenreControllerTest(GameStoreApiFactory factory)
        {
            client = factory.CreateClient();
        }


        [Fact]
        public async Task CreateGenreAsync_ShouldReturnCreated_AndCorrectStatusCode()
        {
            var request = new CreateGenreRequest
            {
                Genre = new CreateGenreDto { Name = "Strategy - " + Guid.NewGuid().ToString() }
            };

            var response = await client.PostAsJsonAsync("/api/genres", request);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.NotNull(content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateGenreAsync_ShouldReturnNoContent_WhenGenreIsUpdated()
        {
            var request = new CreateGenreRequest
            {
                Genre = new CreateGenreDto { Name = "Strategy - " + Guid.NewGuid().ToString() }
            };
            var createResponse = await client.PostAsJsonAsync("/api/genres", request);
            createResponse.EnsureSuccessStatusCode();

            var createdGenre = await createResponse.Content.ReadFromJsonAsync<GenreDto>();

            var updateRequest = new UpdateGenreRequest
            {
                Genre=new GenreDto
                {
                    Id = createdGenre!.Id,
                    Name = "Updated Genre"
                }
            };

            var response = await client.PutAsJsonAsync("/api/genres", updateRequest);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }


        [Fact]
        public async Task DeleteGenreAsync_ShouldReturnNotFound_WhenGenreDoesNotExist()
        {
            var nonExistentGenreId = Guid.NewGuid();
            var response = await client.DeleteAsync($"/api/genres/{nonExistentGenreId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteGenreAsync_ShouldReturnNoContent_WhenGenreIsDeleted()
        {
            var createRequest = new CreateGenreRequest { Genre=new CreateGenreDto { Name = "Genre to Delete" } };
            var createResponse = await client.PostAsJsonAsync("/api/genres", createRequest);
            var createdGenre = await createResponse.Content.ReadFromJsonAsync<GenreDto>();

            var deleteResponse = await client.DeleteAsync($"/api/genres/{createdGenre!.Id}");

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }


        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnNotFound_WhenGenreDoesNotExist()
        {
            var nonExistentGenreId = Guid.NewGuid();
            var response = await client.GetAsync($"/api/genres/{nonExistentGenreId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnOk_AndGenre()
        {
            var createRequest = new CreateGenreRequest { Genre = new CreateGenreDto { Name = "Genre to Get by ID" } };
            var createResponse = await client.PostAsJsonAsync("/api/genres", createRequest);
            var createdGenre = await createResponse.Content.ReadFromJsonAsync<GenreDto>();

            var getResponse = await client.GetAsync($"/api/genres/{createdGenre!.Id}");
            getResponse.EnsureSuccessStatusCode();

            var genre = await getResponse.Content.ReadFromJsonAsync<GenreDto>();

            Assert.NotNull(genre);
            Assert.Equal(createdGenre.Id, genre.Id);
        }


        [Fact]
        public async Task GetAllGenresAsync_ShouldReturnOk_AndListOfGenres()
        {
            var response = await client.GetAsync("/api/genres");
            response.EnsureSuccessStatusCode();

            var genres = await response.Content.ReadFromJsonAsync<List<GenreDto>>();

            Assert.NotNull(genres);
            Assert.IsType<List<GenreDto>>(genres);
        }


        [Fact]
        public async Task GetGenreByParentIdAsync_ShouldReturnNotFound_WhenGenreDoesNotExist()
        {
            var nonExistentParentId = Guid.NewGuid();
            var response = await client.GetAsync($"/api/genres/{nonExistentParentId}/genres");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGenreByParentIdAsync_ShouldReturnOk_AndListOfGenres()
        {
            var parentRequest = new CreateGenreRequest { Genre = new CreateGenreDto { Name = "Parent Genre" } };
            var parentResponse = await client.PostAsJsonAsync("/api/genres", parentRequest);
            var parentGenre = await parentResponse.Content.ReadFromJsonAsync<GenreDto>();

            
            var getResponse = await client.GetAsync($"/api/genres/{parentGenre!.Id}/genres");
            getResponse.EnsureSuccessStatusCode();

            var subGenres = await getResponse.Content.ReadFromJsonAsync<List<GenreDto>>();

            Assert.NotNull(subGenres);
            Assert.IsType<List<GenreDto>>(subGenres);
        }


        [Fact]
        public async Task GetGameByGenreAsync_ShouldReturnNotFound_WhenGenreDoesNotExist()
        {
            var nonExistentGenreId = Guid.NewGuid();
            var response = await client.GetAsync($"/api/genres/{nonExistentGenreId}/games");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGameByGenreAsync_ShouldReturnOk_AndListOfGames()
        {
            var createRequest = new CreateGenreRequest { Genre = new CreateGenreDto { Name = "Genre for Games" } };
            var createResponse = await client.PostAsJsonAsync("/api/genres", createRequest);
            var createdGenre = await createResponse.Content.ReadFromJsonAsync<GenreDto>();

            var getResponse = await client.GetAsync($"/api/genres/{createdGenre!.Id}/games");
            getResponse.EnsureSuccessStatusCode();

            var games = await getResponse.Content.ReadFromJsonAsync<List<GameDto>>();

            Assert.NotNull(games);
            Assert.IsType<List<GameDto>>(games);
        }
    }
}