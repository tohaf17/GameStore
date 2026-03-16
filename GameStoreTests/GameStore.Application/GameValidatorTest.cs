using System;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class GameValidatorTest
    {
        private readonly GameValidator validator;

        public GameValidatorTest()
        {
            validator = new GameValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new GameDto
            {
                Id = Guid.NewGuid(),
                Name = "The Witcher 3",
                Key = "the-witcher-3",
                Description = "A fantastic open world RPG game."
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveErrorWhenIdIsEmpty()
        {
            var model = new GameDto { Id = Guid.Empty };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("Game ID is required.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenNameIsEmpty(string? invalidName)
        {
            var model = new GameDto { Name = invalidName! };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Game name is required.");
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsTooShort()
        {
            var model = new GameDto { Name = "ab" };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Game name must be at least 3 characters long.");
        }

        [Fact]
        public void ShouldHaveErrorWhenNameExceedsMaxLength()
        {
            var model = new GameDto { Name = new string('a', 101) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Game name must not exceed 100 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenKeyIsEmpty(string? invalidKey)
        {
            var model = new GameDto { Key = invalidKey! };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Key)
                  .WithErrorMessage("Game key is required.");
        }

        [Theory]
        [InlineData("The-Witcher-3")]
        [InlineData("the_witcher_3")]
        [InlineData("the witcher 3")]
        [InlineData("the-witcher-3!")]
        public void ShouldHaveErrorWhenKeyDoesNotMatchRegex(string invalidKey)
        {
            var model = new GameDto { Key = invalidKey };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Key)
                  .WithErrorMessage("Game key must contain only lowercase letters, numbers, and hyphens (e.g., 'the-witcher-3').");
        }

        [Fact]
        public void ShouldHaveErrorWhenKeyExceedsMaxLength()
        {
            var model = new GameDto { Key = new string('a', 51) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Key)
                  .WithErrorMessage("Game key must not exceed 50 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenDescriptionIsEmpty(string? invalidDescription)
        {
            var model = new GameDto { Description = invalidDescription! };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Description is required.");
        }

        [Fact]
        public void ShouldHaveErrorWhenDescriptionIsTooShort()
        {
            var model = new GameDto { Description = "Too short" };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Description must be at least 10 characters long.");
        }

        [Fact]
        public void ShouldHaveErrorWhenDescriptionExceedsMaxLength()
        {
            var model = new GameDto { Description = new string('a', 2001) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Description must not exceed 2000 characters.");
        }
    }
}