using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;   

namespace GameStore.Application.Requests
{
    public class CreateGenreRequest
    {
        public required CreateGenreDto Genre { get; set; }

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
