using System;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class GenreValidatorTest
    {
        private readonly GenreValidator validator;

        public GenreValidatorTest()
        {
            validator = new GenreValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = "Action",
                ParentGenreId = Guid.NewGuid()
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenParentGenreIdIsNull()
        {
            var model = new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = "Action",
                ParentGenreId = null
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveErrorWhenIdIsEmpty()
        {
            var model = new GenreDto
            {
                Id = Guid.Empty,
                Name = "Action"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("Genre ID is required.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenNameIsEmpty(string? invalidName)
        {
            var model = new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = invalidName!
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Genre name is required.");
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsTooShort()
        {
            var model = new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = "A"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Genre name must be at least 2 characters long.");
        }

        [Fact]
        public void ShouldHaveErrorWhenNameExceedsMaxLength()
        {
            var model = new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = new string('a', 51)
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Genre name must not exceed 50 characters.");
        }

        [Fact]
        public void ShouldHaveErrorWhenParentGenreIdEqualsId()
        {
            var id = Guid.NewGuid();
            var model = new GenreDto
            {
                Id = id,
                Name = "Action",
                ParentGenreId = id
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ParentGenreId)
                  .WithErrorMessage("A genre cannot be its own parent.");
        }
    }
}