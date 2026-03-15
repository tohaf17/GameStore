using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;
using FluentValidation.TestHelper;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class CreateGenreValidatorTest
    {
        private readonly CreateGenreValidator validator;
        public CreateGenreValidatorTest()
        {
            validator = new CreateGenreValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenNameIsEmpty(string invalidName)
        {
            var model = new CreateGenreDto { Name = invalidName };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Name)
                  .WithErrorMessage("Name is required");
        }
        [Fact]
        public void ShouldHaveErrorWhenNameExceedsMaxLength()
        {
            var model = new CreateGenreDto { Name = new string('a', 101) };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Name)
                  .WithErrorMessage("Name cannot exceed 100 characters");
        }
    }
}
