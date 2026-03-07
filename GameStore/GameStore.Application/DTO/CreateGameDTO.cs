using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace GameStore.Application.DTO
{
    public class CreateGameDto
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class CreateGameValidator:AbstractValidator<CreateGameDto>
    {
        public CreateGameValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
            
            RuleFor(g=>g.Key)
                .NotEmpty().WithMessage("Key is required")
                .MaximumLength(100).WithMessage("Key cannot exceed 100 characters")
                .Matches("^[a-z0-9]+(-[a-z0-9]+)*$").WithMessage("Key must be lowercase and can contain hyphens");

            RuleFor(g => g.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        }
    }
}
