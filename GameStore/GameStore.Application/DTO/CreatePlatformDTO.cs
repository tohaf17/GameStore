using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace GameStore.Application.DTO
{
    public class CreatePlatformDto
    {
        public string Type { get; set; }
    }
    public class CreatePlatformValidator : AbstractValidator<CreatePlatformDto>
    {
        public CreatePlatformValidator()
        {
            RuleFor(p => p.Type)
                .NotEmpty().WithMessage("Type is required")
                .MaximumLength(100).WithMessage("Type cannot exceed 100 characters");
        }
    }
}
