using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class PlatformDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
    public class PlatformValidator : AbstractValidator<PlatformDto>
    {
        public PlatformValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Platform ID is required.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Platform type is required.")
                .MaximumLength(50)
                .WithMessage("Platform type must not exceed 50 characters.")
                .MinimumLength(2)
                .WithMessage("Platform type must be at least 2 characters long.");
        }
    }
}
