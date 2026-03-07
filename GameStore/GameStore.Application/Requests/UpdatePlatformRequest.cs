using FluentValidation;
using GameStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameStore.Application.Requests
{
    public class UpdatePlatformRequest
    {
        public required PlatformDto Platform { get; set; }
    }
    public class UpdatePlatformRequestValidator : AbstractValidator<UpdatePlatformRequest>
    {
        public UpdatePlatformRequestValidator()
        {
            RuleFor(x => x.Platform)
                .NotNull()
                .WithMessage("Platform data is required.")
                .SetValidator(new PlatformValidator());

            RuleFor(x => x.Platform.Id)
                .NotEmpty()
                .WithMessage("Platform ID is required for the update operation.");
        }
    }
}
