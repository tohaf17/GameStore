using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.TestHelper;
using GameStore.Application.DTO;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class CreatePlatformValidatorTest
    {
        private readonly CreatePlatformValidator validator;

        public CreatePlatformValidatorTest()
        {
            validator = new CreatePlatformValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenTypeIsEmpty(string? type)
        {
            var model = new CreatePlatformDto { Type = type! };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.Type)
                .WithErrorMessage("Type is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenTypeExceedsMaxLength()
        {
            var model = new CreatePlatformDto { Type = new string('a', 101) };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.Type)
                .WithErrorMessage("Type cannot exceed 100 characters");
        }
    }
}
