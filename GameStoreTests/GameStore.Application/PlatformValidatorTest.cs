using System;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class PlatformValidatorTest
    {
        private readonly PlatformValidator validator;

        public PlatformValidatorTest()
        {
            validator = new PlatformValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new PlatformDto
            {
                Id = Guid.NewGuid(),
                Type = "PC"
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveErrorWhenIdIsEmpty()
        {
            var model = new PlatformDto
            {
                Id = Guid.Empty,
                Type = "PC"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("Platform ID is required.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenTypeIsEmpty(string invalidType)
        {
            var model = new PlatformDto
            {
                Id = Guid.NewGuid(),
                Type = invalidType
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Type)
                  .WithErrorMessage("Platform type is required.");
        }

        [Fact]
        public void ShouldHaveErrorWhenTypeIsTooShort()
        {
            var model = new PlatformDto
            {
                Id = Guid.NewGuid(),
                Type = "A"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Type)
                  .WithErrorMessage("Platform type must be at least 2 characters long.");
        }

        [Fact]
        public void ShouldHaveErrorWhenTypeExceedsMaxLength()
        {
            var model = new PlatformDto
            {
                Id = Guid.NewGuid(),
                Type = new string('a', 51)
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Type)
                  .WithErrorMessage("Platform type must not exceed 50 characters.");
        }
    }
}