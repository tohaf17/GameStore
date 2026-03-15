using FluentValidation;
using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdateGameRequest
    {
        public required GameDto Game { get; set; }
        public required ICollection<GenreDto> Genres { get; set; }
        public required ICollection<PlatformDto> Platforms { get; set; }
    }
    public class UpdateGameRequestValidator : AbstractValidator<UpdateGameRequest>
    {
        public UpdateGameRequestValidator()
        {
            RuleFor(x => x.Game)
                .NotNull()
                .SetValidator(new GameValidator()); 

            RuleForEach(x => x.Genres).ChildRules(genre =>
            {
                genre.RuleFor(g => g.Id).NotEmpty().WithMessage("Genre ID is required");
            });
            RuleForEach(x => x.Platforms).ChildRules(platform =>
            {
                platform.RuleFor(p => p.Id).NotEmpty().WithMessage("Platform ID is required");
            });
        }
    }
}
