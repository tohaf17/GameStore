using System;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class CreateGameRequestValidatorTest
    {
        private readonly CreateGameRequestValidator validator;

        public CreateGameRequestValidatorTest()
        {
            validator = new CreateGameRequestValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Name = "Valid Name",
                    Key = "valid-key",
                    Description = "Valid description string."
                },
                Genres = new List<GenreDto>
                {
                    new GenreDto { Id = Guid.NewGuid(), Name = "Action" }
                },
                Platforms = new List<PlatformDto>
                {
                    new PlatformDto { Id = Guid.NewGuid(), Type = "PC" }
                }
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveErrorWhenGameIsNull()
        {
            var model = new CreateGameRequest
            {
                Game = null!,
                Genres = new List<GenreDto>
                {
                    new GenreDto { Id = Guid.NewGuid(), Name = "Action" }
                },
                Platforms = new List<PlatformDto>()
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.Game)
                  .WithErrorMessage("Game information is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenGenresCollectionIsEmpty()
        {
            var model = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Name = "Valid Name",
                    Key = "valid-key",
                    Description = "Valid description string."
                },
                Genres = new List<GenreDto>(),
                Platforms = new List<PlatformDto>()
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.Genres)
                  .WithErrorMessage("At least one genre must be selected");
        }

        [Fact]
        public void ShouldHaveErrorWhenAnyGenreIdIsEmpty()
        {
            var model = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Name = "Valid Name",
                    Key = "valid-key",
                    Description = "Valid description string."
                },
                Genres = new List<GenreDto>
                {
                    new GenreDto { Id = Guid.Empty, Name = "Action" }
                },
                Platforms = new List<PlatformDto>()
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor("Genres[0].Id")
                  .WithErrorMessage("Genre ID is missing");
        }

        [Fact]
        public void ShouldHaveErrorWhenAnyPlatformIdIsEmpty()
        {
            var model = new CreateGameRequest
            {
                Game = new CreateGameDto
                {
                    Name = "Valid Name",
                    Key = "valid-key",
                    Description = "Valid description string."
                },
                Genres = new List<GenreDto>
                {
                    new GenreDto { Id = Guid.NewGuid(), Name = "Action" }
                },
                Platforms = new List<PlatformDto>
                {
                    new PlatformDto { Id = Guid.Empty, Type = "PC" }
                }
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor("Platforms[0].Id")
                  .WithErrorMessage("Platform ID is missing");
        }
    }
}