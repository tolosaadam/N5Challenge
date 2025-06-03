using FluentValidation;
using N5Challenge.Api.Application.Constants;
using N5Challenge.Api.Application.Permission.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Create;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(m => m.EmployeeLastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ValidationMessagesConstants.Empty)
                .NotNull()
                    .WithMessage(ValidationMessagesConstants.Null);

        RuleFor(m => m.EmployeeFirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ValidationMessagesConstants.Empty)
                .NotNull()
                    .WithMessage(ValidationMessagesConstants.Null);

        RuleFor(m => m.PermissionTypeId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ValidationMessagesConstants.Empty)
                .NotNull()
                    .WithMessage(ValidationMessagesConstants.Null);
    }
}
