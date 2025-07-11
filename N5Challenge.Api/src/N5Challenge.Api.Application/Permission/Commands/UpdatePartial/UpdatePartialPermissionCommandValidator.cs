﻿using FluentValidation;
using N5Challenge.Api.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.UpdatePartial;

public class UpdatePartialPermissionCommandValidator : AbstractValidator<UpdatePartialPermissionCommand>
{
    public UpdatePartialPermissionCommandValidator()
    {
        RuleFor(m => m.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ValidationMessagesConstants.Empty)
                .NotNull()
                    .WithMessage(ValidationMessagesConstants.Null);
    }
}