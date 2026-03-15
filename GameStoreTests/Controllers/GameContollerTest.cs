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
        public GameContollerTest(GameStoreApiFactory factory)
        {
            client = factory.CreateClient();
        }

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
            // 1. Arrange - Створюємо гру, щоб вона фізично існувала в InMemory базі
            var uniqueKey = "game-" + Guid.NewGuid();
            var createRequest = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Key = uniqueKey,
                    Name = "Original Name",
                    Description = "Original Description"
                },
                Genres = new List<GenreDto>(),
                Platforms = new List<PlatformDto>()
            };

            var createResponse = await client.PostAsJsonAsync("/api/games", createRequest);
            createResponse.EnsureSuccessStatusCode();

            var createdGame = await createResponse.Content.ReadFromJsonAsync<GameDto>();
            Assert.NotNull(createdGame);
            var gameId = createdGame.Id;

            // 2. Prepare Update - Створюємо об'єкт для оновлення
            // ВАЖЛИВО: Використовуємо той самий ID, але змінюємо дані
            var updateRequest = new UpdateGameRequest
            {
                Game = new GameDto
                {
                    Id = gameId,
                    Key = uniqueKey, // Залишаємо той самий ключ або міняємо, якщо сервіс дозволяє
                    Name = "Updated Name",
                    Description = "Updated Description"
                },
                Genres = new List<GenreDto>(),
                Platforms = new List<PlatformDto>()
            };

            // 3. Act - Відправляємо PUT запит
            // Використовуємо шлях /api/games, бо твій контролер не приймає ID в URL
            var response = await client.PutAsJsonAsync("/api/games", updateRequest);

            // Допомога в дебазі, якщо знову буде 500
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Update failed! Status: {response.StatusCode}. Error: {errorContent}");
            }

            // 4. Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // 5. Verify - Перевіряємо, чи дані дійсно змінилися в базі
            // Отримуємо список ігор і шукаємо нашу за ID
            var getResponse = await client.GetAsync("/api/games");
            var games = await getResponse.Content.ReadFromJsonAsync<List<GameDto>>();
            var updatedGame = games?.FirstOrDefault(g => g.Id == gameId);

            Assert.NotNull(updatedGame);
            Assert.Equal("Updated Name", updatedGame.Name);
            Assert.Equal("Updated Description", updatedGame.Description);
        }
    }
    }
