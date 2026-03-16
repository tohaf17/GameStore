using GameStore.Application.DTO;
using GameStore.Application.Requests;
using System;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace GameStoreTests.Controllers
{
    public class GameContollerTest : IClassFixture<GameStoreApiFactory>
    {
        private readonly HttpClient client;
        //public GameContollerTest(GameStoreApiFactory factory)
        //{
        //    client = factory.CreateClient();
        //}

        [Fact]
        public async Task CreateGameAsync_ShouldReturnNotNull_AndCorrectStatusCode()
        {
            var request = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = "key",
                    Name = "game_name",
                    Description = "game_description"
                },

                Genres = new List<GenreDto>
                {
                    new GenreDto { Id = Guid.NewGuid(), Name = "RPG" }
                },

                Platforms = new List<PlatformDto>
                {
                    new PlatformDto { Id = Guid.NewGuid(), Type = "PC" }
                }
            };
            var response = await client.PostAsJsonAsync("/api/games", request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.NotNull(content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateGameAsync_ShouldReturnNoContent_WhenGameIsUpdated()
        {
            var uniqueKey = ("game-" + Guid.NewGuid()).ToLower();

            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Original Name",
                    Description = "Original Description"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };

            var createResponse = await client.PostAsJsonAsync("/api/games", createRequest);

            var createdGame = await createResponse.Content.ReadFromJsonAsync<GameDto>();
            var gameId = createdGame.Id;

            var getResponse = await client.GetAsync($"/api/games/{gameId}");
            if (!getResponse.IsSuccessStatusCode)
            {
                var getError = await getResponse.Content.ReadAsStringAsync();
                throw new Exception($"Не можу знайти створену гру! Статус GET: {getResponse.StatusCode}. Тіло: {getError}");
            }

            var updateRequest = new UpdateGameRequest
            {
                Game = new GameDto
                {
                    Id = gameId,
                    Key = uniqueKey,
                    Name = "Updated Name",
                    Description = "Updated Description"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var response = await client.PutAsJsonAsync("/api/games", updateRequest);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }
        [Fact]
        public void DeleteGameAsync_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            var nonExistentGameId = Guid.NewGuid();
            var response = client.DeleteAsync($"/api/games/{nonExistentGameId}").Result;
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void DeleteGameAsync_ShouldReturnNoContent_WhenGameIsDeleted()
        {
            var uniqueKey = ("game-" + Guid.NewGuid()).ToLower();
            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Game to Delete",
                    Description = "This game will be deleted"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var createResponse = client.PostAsJsonAsync("/api/games", createRequest).Result;
            var createdGame = createResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            var gameId = createdGame.Id;
            var deleteResponse = client.DeleteAsync($"/api/games/{gameId}").Result;
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        }

        [Fact]
        public void GetGameFilesAsync_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            var nonExistentGameId = Guid.NewGuid();
            var response = client.GetAsync($"/api/games/{nonExistentGameId}/files").Result;
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void GetGameFilesAsync_ShouldReturnOk_WhenGameExists()
        {
            var uniqueKey = ("game-" + Guid.NewGuid()).ToLower();
            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Game with Files",
                    Description = "This game will have files"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var createResponse = client.PostAsJsonAsync("/api/games", createRequest).Result;
            var createdGame = createResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            var gameId = createdGame.Id;
            var getFilesResponse = client.GetAsync($"/api/games/{gameId}/files").Result;
            Assert.Equal(HttpStatusCode.OK, getFilesResponse.StatusCode);
        }
        [Fact]
        public void GetAllGamesAsync_ShouldReturnOk_AndListOfGames()
        {
            var response = client.GetAsync("/api/games").Result;
            response.EnsureSuccessStatusCode();
            var games = response.Content.ReadFromJsonAsync<List<GameDto>>().Result;
            Assert.NotNull(games);
            Assert.IsType<List<GameDto>>(games);
        }
        [Fact]
        public void GetGameByKeyAsync_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            var nonExistentKey = "non-existent-key";
            var response = client.GetAsync($"/api/games/{nonExistentKey}").Result;
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void GetGameByKeyAsync_ShouldReturnOk_AndGame()
        {
            var uniqueKey = ("game-" + Guid.NewGuid()).ToLower();
            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Game to Get",
                    Description = "This game will be retrieved by key"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var createResponse = client.PostAsJsonAsync("/api/games", createRequest).Result;
            var createdGame = createResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            var getResponse = client.GetAsync($"/api/games/{uniqueKey}").Result;
            getResponse.EnsureSuccessStatusCode();
            var game = getResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            Assert.NotNull(game);
            Assert.Equal(uniqueKey, game.Key);
        }
        [Fact]
        public void GetGameGenresByKeyAsync_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            var nonExistentKey = "non-existent-key";
            var response = client.GetAsync($"/api/games/{nonExistentKey}/genres").Result;
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void GetGameGenresByKeyAsync_ShouldReturnOk_AndListOfGenres()
        {
            var uniqueKey = ("game-" + Guid.NewGuid()).ToLower();
            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Game with Genres",
                    Description = "This game will have genres"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var createResponse = client.PostAsJsonAsync("/api/games", createRequest).Result;
            var createdGame = createResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            var getResponse = client.GetAsync($"/api/games/{uniqueKey}/genres").Result;
            getResponse.EnsureSuccessStatusCode();
            var genres = getResponse.Content.ReadFromJsonAsync<List<GenreDto>>().Result;
            Assert.NotNull(genres);
            Assert.IsType<List<GenreDto>>(genres);
        }
        [Fact]
        public void GetGamePlatformsByKeyAsync_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            var nonExistentKey = "non-existent-key";
            var response = client.GetAsync($"/api/games/{nonExistentKey}/platforms").Result;
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void GetGamePlatformsByKeyAsync_ShouldReturnOk_AndListOfPlatforms()
        {
            var uniqueKey = ("game-" + Guid.NewGuid()).ToLower();
            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Game with Platforms",
                    Description = "This game will have platforms"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var createResponse = client.PostAsJsonAsync("/api/games", createRequest).Result;
            var createdGame = createResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            var getResponse = client.GetAsync($"/api/games/{uniqueKey}/platforms").Result;
            getResponse.EnsureSuccessStatusCode();
            var platforms = getResponse.Content.ReadFromJsonAsync<List<PlatformDto>>().Result;
            Assert.NotNull(platforms);
            Assert.IsType<List<PlatformDto>>(platforms);
        }
        [Fact]
        public void GetGameByIdAsync_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            var nonExistentGameId = Guid.NewGuid();
            var response = client.GetAsync($"/api/games/{nonExistentGameId}").Result;
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void GetGameByIdAsync_ShouldReturnOk_AndGame()
        {
            var uniqueKey = ("game-" + Guid.NewGuid()).ToLower();
            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Game to Get by ID",
                    Description = "This game will be retrieved by ID"
                },
                Genres = new List<GenreDto> { new GenreDto { Id = Guid.NewGuid(), Name = "Action" } },
                Platforms = new List<PlatformDto> { new PlatformDto { Id = Guid.NewGuid(), Type = "PC" } }
            };
            var createResponse = client.PostAsJsonAsync("/api/games", createRequest).Result;
            var createdGame = createResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            var gameId = createdGame.Id;
            var getResponse = client.GetAsync($"/api/games/{gameId}").Result;
            getResponse.EnsureSuccessStatusCode();
            var game = getResponse.Content.ReadFromJsonAsync<GameDto>().Result;
            Assert.NotNull(game);
            Assert.Equal(gameId, game.Id);
        }
    }
}