using DailyYield.Application.Commands;
using FluentValidation;

namespace DailyYield.Application.Validators;

public class CreateUserGroupCommandValidator : AbstractValidator<CreateUserGroupCommand>
{
    public CreateUserGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("User group name is required")
            .MinimumLength(2).WithMessage("User group name must be at least 2 characters")
            .MaximumLength(100).WithMessage("User group name must not exceed 100 characters");

        RuleFor(x => x.Timezone)
            .NotEmpty().WithMessage("Timezone is required")
            .MaximumLength(50).WithMessage("Timezone must not exceed 50 characters");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required");
    }
}