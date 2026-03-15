using System;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class UpdateGenreRequestValidatorTest
    {
        private readonly UpdateGenreRequestValidator validator;

        public UpdateGenreRequestValidatorTest()
        {
            validator = new UpdateGenreRequestValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new UpdateGenreRequest
            {
                Genre = new GenreDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Action",
                    ParentGenreId = null
                }
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveErrorWhenGenreIsNull()
        {
            var model = new UpdateGenreRequest
            {
                Genre = null!
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Genre)
                  .WithErrorMessage("Genre data must be provided.");
        }

        [Fact]
        public void ShouldHaveErrorWhenGenreIdIsEmpty()
        {
            var model = new UpdateGenreRequest
            {
                Genre = new GenreDto
                {
                    Id = Guid.Empty,
                    Name = "Action"
                }
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Genre.Id)
                  .WithErrorMessage("Genre ID is mandatory for update operations.");
        }
    }
}