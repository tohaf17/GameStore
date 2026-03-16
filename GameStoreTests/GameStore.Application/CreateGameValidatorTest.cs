using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.TestHelper;
using Xunit;

namespace GameStoreTests.GameStore.Application
{
    public class CreateGameValidatorTest
    {
        private readonly CreateGameValidator validator;

        public CreateGameValidatorTest()
        {
            validator = new CreateGameValidator();
        }
        [Fact]
        public void ShouldNotHaveErrorWhenModelIsValid()
        {
            var model = new CreateGameDto
            {
                Name = "The Witcher 3",
                Key = "the-witcher-3-wild-hunt",
                Description = "Awesome RPG game"
            };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenNameIsEmpty(string? invalidName)
        {
            var model = new CreateGameDto { Name = invalidName! };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Name)
                  .WithErrorMessage("Name is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenNameExceedsMaxLength()
        {
            var model = new CreateGameDto { Name = new string('a', 101) };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Name)
                  .WithErrorMessage("Name cannot exceed 100 characters");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenKeyIsEmpty(string? invalidKey)
        {
            var model = new CreateGameDto { Key = invalidKey! };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Key)
                  .WithErrorMessage("Key is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenKeyExceedsMaxLength()
        {
            var model = new CreateGameDto { Key = new string('a', 101) };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Key)
                  .WithErrorMessage("Key cannot exceed 100 characters");
        }
        [Theory]
        [InlineData("asasSASASPIA()#))DADA")] 
        [InlineData("the witcher")]         
        [InlineData("the_witcher")]           
        [InlineData("-thewitcher")]           
        [InlineData("thewitcher-")]
        public void ShouldHaveErrorWhenKeyIsNotLowercase(string invalidKey)
        {
            var model = new CreateGameDto { Key = invalidKey };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Key)
                  .WithErrorMessage("Key must be lowercase and can contain hyphens");
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveErrorWhenDescriptionIsEmpty(string? invalidDescription)
        {
            var model = new CreateGameDto { Description = invalidDescription! };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Description)
                  .WithErrorMessage("Description is required");
        }
        [Fact]
        public void ShouldHaveErrorWhenDescriptionExceedsMaxLength()
        {
            var model = new CreateGameDto { Description = new string('a', 1001) };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(g => g.Description)
                  .WithErrorMessage("Description cannot exceed 1000 characters");
        }
    }
}