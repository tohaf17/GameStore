using FluentValidation;
using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdateGenreRequest
    {
        public GenreDto Genre { get; set; } = default!;

    }
    public class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
    {
        public UpdateGenreRequestValidator()
        {
            RuleFor(x => x.Genre)
                .NotNull()
                .WithMessage("Genre data must be provided.")
                .SetValidator(new GenreValidator());

            When(x => x.Genre != null, () =>
            {
                RuleFor(x => x.Genre.Id)
                    .NotEmpty()
                    .WithMessage("Genre ID is mandatory for update operations.");
            });
        }
    }

}
