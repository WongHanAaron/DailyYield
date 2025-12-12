using DailyYield.Application.Commands;
using FluentValidation;

namespace DailyYield.Application.Validators;

public class CreateGoalCommandValidator : AbstractValidator<CreateGoalCommand>
{
    public CreateGoalCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.MetricTypeId)
            .NotEmpty().WithMessage("Metric type ID is required");

        RuleFor(x => x.TargetValue)
            .GreaterThan(0).WithMessage("Target value must be greater than 0");

        RuleFor(x => x.Timeframe)
            .IsInEnum().WithMessage("Invalid timeframe");

        RuleFor(x => x.GoalType)
            .IsInEnum().WithMessage("Invalid goal type");

        RuleFor(x => x.Frequency)
            .MaximumLength(100).WithMessage("Frequency must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Frequency));

        RuleFor(x => x.Comparison)
            .IsInEnum().WithMessage("Invalid comparison");
    }
}

public class UpdateGoalCommandValidator : AbstractValidator<UpdateGoalCommand>
{
    public UpdateGoalCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Goal ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.TargetValue)
            .GreaterThan(0).WithMessage("Target value must be greater than 0");

        RuleFor(x => x.Timeframe)
            .IsInEnum().WithMessage("Invalid timeframe");

        RuleFor(x => x.GoalType)
            .IsInEnum().WithMessage("Invalid goal type");

        RuleFor(x => x.Frequency)
            .MaximumLength(100).WithMessage("Frequency must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Frequency));

        RuleFor(x => x.Comparison)
            .IsInEnum().WithMessage("Invalid comparison");
    }
}

public class DeleteGoalCommandValidator : AbstractValidator<DeleteGoalCommand>
{
    public DeleteGoalCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Goal ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}