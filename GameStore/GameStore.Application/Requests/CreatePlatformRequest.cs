using System;
using System.Collections.Generic;
using System.Text;
using GameStore.Application.DTO;

namespace GameStore.Application.Requests
{
    public class CreatePlatformRequest
    {
        public required CreatePlatformDto Platform { get; set; }
    }
    public class CreatePlatformRequestValidator : FluentValidation.AbstractValidator<CreatePlatformRequest>
    {
        public CreatePlatformRequestValidator()
        {
            RuleFor(r => r.Platform)
                .SetValidator(new CreatePlatformValidator());
        }
    }
    }
