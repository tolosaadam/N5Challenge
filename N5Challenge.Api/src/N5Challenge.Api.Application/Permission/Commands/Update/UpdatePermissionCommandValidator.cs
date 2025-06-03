using FluentValidation;
using N5Challenge.Api.Application.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Update;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(m => m.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                    .WithMessage(ValidationMessagesConstants.GreaterThanZero)
                .NotEmpty()
                    .WithMessage(ValidationMessagesConstants.Empty)
                .NotNull()
                    .WithMessage(ValidationMessagesConstants.Null);
    }
}