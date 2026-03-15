using System;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class UpdateGameRequestValidatorTest
    {
        private readonly UpdateGameRequestValidator validator;

        public UpdateGameRequestValidatorTest()
        {
            validator = new UpdateGameRequestValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new UpdateGameRequest
            {
                Game = new GameDto
                {
                    Id = Guid.NewGuid(),
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
            var model = new UpdateGameRequest
            {
                Game = null!,
                Genres = new List<GenreDto>
                {
                    new GenreDto { Id = Guid.NewGuid(), Name = "Action" }
                },
                Platforms = new List<PlatformDto>()
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Game);
        }

        [Fact]
        public void ShouldHaveErrorWhenAnyGenreIdIsEmpty()
        {
            var model = new UpdateGameRequest
            {
                Game = new GameDto
                {
                    Id = Guid.NewGuid(),
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
                  .WithErrorMessage("Genre ID is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenAnyPlatformIdIsEmpty()
        {
            var model = new UpdateGameRequest
            {
                Game = new GameDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Valid Name",
                    Key = "valid-key",
                    Description = "Valid description string."
                },
                Genres = new List<GenreDto>(),
                Platforms = new List<PlatformDto>
                {
                    new PlatformDto { Id = Guid.Empty, Type = "PC" }
                }
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor("Platforms[0].Id")
                  .WithErrorMessage("Platform ID is required");
        }
    }
}