using DailyYield.Application.Commands;
using FluentValidation;

namespace DailyYield.Application.Validators;

public class CreateMetricTypeCommandValidator : AbstractValidator<CreateMetricTypeCommand>
{
    public CreateMetricTypeCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required")
            .MinimumLength(2).WithMessage("Key must be at least 2 characters")
            .MaximumLength(50).WithMessage("Key must not exceed 50 characters")
            .Matches("^[a-zA-Z][a-zA-Z0-9_-]*$").WithMessage("Key must start with a letter and contain only letters, numbers, underscores, and hyphens");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .MinimumLength(2).WithMessage("Display name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Display name must not exceed 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid metric data type");

        RuleFor(x => x.Unit)
            .MaximumLength(20).WithMessage("Unit must not exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Unit));

        RuleFor(x => x.UserGroupId)
            .NotEmpty().WithMessage("User group ID is required");
    }
}

public class UpdateMetricTypeCommandValidator : AbstractValidator<UpdateMetricTypeCommand>
{
    public UpdateMetricTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Metric type ID is required");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required")
            .MinimumLength(2).WithMessage("Key must be at least 2 characters")
            .MaximumLength(50).WithMessage("Key must not exceed 50 characters")
            .Matches("^[a-zA-Z][a-zA-Z0-9_-]*$").WithMessage("Key must start with a letter and contain only letters, numbers, underscores, and hyphens");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .MinimumLength(2).WithMessage("Display name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Display name must not exceed 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid metric data type");

        RuleFor(x => x.Unit)
            .MaximumLength(20).WithMessage("Unit must not exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Unit));
    }
}

public class DeleteMetricTypeCommandValidator : AbstractValidator<DeleteMetricTypeCommand>
{
    public DeleteMetricTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Metric type ID is required");
    }
}