using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;   

namespace GameStore.Application.Requests
{
    public class CreateGenreRequest
    {
        public CreateGenreDto Genre { get; set; } = default!;

    }
    public class CreateGenreRequestValidator : FluentValidation.AbstractValidator<CreateGenreRequest>
    {
        public CreateGenreRequestValidator()
        {
            RuleFor(r => r.Genre)
                .SetValidator(new CreateGenreValidator());
        }
    }
    }
