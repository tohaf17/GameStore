using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
    }
    public class GameValidator : AbstractValidator<GameDto>
    {
        public GameValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Game ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Game name is required.")
                .MaximumLength(100).WithMessage("Game name must not exceed 100 characters.")
                .MinimumLength(3).WithMessage("Game name must be at least 3 characters long.");

            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Game key is required.")
                .Matches(@"^[a-z0-9-]+$").WithMessage("Game key must contain only lowercase letters, numbers, and hyphens (e.g., 'the-witcher-3').")
                .MaximumLength(50).WithMessage("Game key must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
        }
    }
}
