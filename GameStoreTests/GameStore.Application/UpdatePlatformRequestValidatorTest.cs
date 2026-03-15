using System;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using GameStore.Application.Requests;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class UpdatePlatformRequestValidatorTest
    {
        private readonly UpdatePlatformRequestValidator validator;

        public UpdatePlatformRequestValidatorTest()
        {
            validator = new UpdatePlatformRequestValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new UpdatePlatformRequest
            {
                Platform = new PlatformDto
                {
                    Id = Guid.NewGuid(),
                    Type = "PC"
                }
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveErrorWhenPlatformIsNull()
        {
            var model = new UpdatePlatformRequest
            {
                Platform = null!
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Platform)
                  .WithErrorMessage("Platform data is required.");
        }

        [Fact]
        public void ShouldHaveErrorWhenPlatformIdIsEmpty()
        {
            var model = new UpdatePlatformRequest
            {
                Platform = new PlatformDto
                {
                    Id = Guid.Empty,
                    Type = "PC"
                }
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Platform.Id)
                  .WithErrorMessage("Platform ID is required for the update operation.");
        }
    }
}