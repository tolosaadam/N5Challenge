using FluentValidation;
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
                    .WithMessage(ValidationMessages.GreaterThanZero)
                .NotEmpty()
                    .WithMessage(ValidationMessages.Empty)
                .NotNull()
                    .WithMessage(ValidationMessages.Null);
    }
}