using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.DTO
{
    public class GenreDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid? ParentGenreId { get; set; }

    }
    public class GenreValidator : AbstractValidator<GenreDto>
    {
        public GenreValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Genre ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Genre name is required.")
                .MaximumLength(50).WithMessage("Genre name must not exceed 50 characters.")
                .MinimumLength(2).WithMessage("Genre name must be at least 2 characters long.");

            RuleFor(x => x.ParentGenreId)
                .Must((genre, parentId) => parentId != genre.Id)
                .WithMessage("A genre cannot be its own parent.");
        }
    }

}
