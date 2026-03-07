using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;
using FluentValidation;

namespace GameStore.Application.Requests
{
    public class CreateGameRequest
    {
        public required CreateGameDto Game { get; set; }

        public required ICollection<GenreDto> Genres { get; set; }
        public required ICollection<PlatformDto> Platforms { get; set; }
    }
    public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
    {
        public CreateGameRequestValidator()
        {
            RuleFor(r => r.Game)
                .NotNull().WithMessage("Game information is required")
                .SetValidator(new CreateGameValidator());

            RuleFor(r => r.Genres).NotEmpty().WithMessage("At least one genre must be selected");

            RuleForEach(r => r.Genres).ChildRules(genre =>
            {
                genre.RuleFor(x => x.Id).NotEmpty().WithMessage("Genre ID is missing");
            });

            RuleForEach(r => r.Platforms).ChildRules(platform =>
            {
                platform.RuleFor(x => x.Id).NotEmpty().WithMessage("Platform ID is missing");
            });
        }
    }
}
