using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace GameStore.Application.DTO
{
    public class CreateGenreDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }

    }
    public class CreateGenreValidator : AbstractValidator<CreateGenreDto>
    {
        public CreateGenreValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
        }
    }
}
