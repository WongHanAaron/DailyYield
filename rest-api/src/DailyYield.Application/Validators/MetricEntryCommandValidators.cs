using DailyYield.Application.Commands;
using FluentValidation;

namespace DailyYield.Application.Validators;

public class CreateMetricEntryCommandValidator : AbstractValidator<CreateMetricEntryCommand>
{
    public CreateMetricEntryCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.MetricTypeId)
            .NotEmpty().WithMessage("Metric type ID is required");

        RuleFor(x => x.NumericValue)
            .GreaterThanOrEqualTo(0).WithMessage("Numeric value must be greater than or equal to 0")
            .When(x => x.NumericValue.HasValue);

        RuleFor(x => x.CategoryValue)
            .MaximumLength(500).WithMessage("Category value must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.CategoryValue));

        RuleFor(x => x.Timestamp)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Timestamp cannot be in the future")
            .When(x => x.Timestamp.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        // At least one value type should be provided
        RuleFor(x => x)
            .Must(x => x.NumericValue.HasValue || x.BooleanValue.HasValue || !string.IsNullOrEmpty(x.CategoryValue))
            .WithMessage("At least one value (numeric, boolean, or category) must be provided");
    }
}

public class UpdateMetricEntryCommandValidator : AbstractValidator<UpdateMetricEntryCommand>
{
    public UpdateMetricEntryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Metric entry ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.NumericValue)
            .GreaterThanOrEqualTo(0).WithMessage("Numeric value must be greater than or equal to 0")
            .When(x => x.NumericValue.HasValue);

        RuleFor(x => x.CategoryValue)
            .MaximumLength(500).WithMessage("Category value must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.CategoryValue));

        RuleFor(x => x.Timestamp)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Timestamp cannot be in the future")
            .When(x => x.Timestamp.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        // At least one value type should be provided
        RuleFor(x => x)
            .Must(x => x.NumericValue.HasValue || x.BooleanValue.HasValue || !string.IsNullOrEmpty(x.CategoryValue))
            .WithMessage("At least one value (numeric, boolean, or category) must be provided");
    }
}

public class DeleteMetricEntryCommandValidator : AbstractValidator<DeleteMetricEntryCommand>
{
    public DeleteMetricEntryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Metric entry ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}