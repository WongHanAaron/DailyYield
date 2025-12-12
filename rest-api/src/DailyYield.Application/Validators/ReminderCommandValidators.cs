using DailyYield.Application.Commands;
using FluentValidation;

namespace DailyYield.Application.Validators;

public class CreateReminderCommandValidator : AbstractValidator<CreateReminderCommand>
{
    public CreateReminderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(1).WithMessage("Title must be at least 1 character")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("Either Task ID or Metric Type ID must be provided")
            .When(x => !x.MetricTypeId.HasValue)
            .WithMessage("Either Task ID or Metric Type ID must be provided");

        RuleFor(x => x.MetricTypeId)
            .NotEmpty().WithMessage("Either Task ID or Metric Type ID must be provided")
            .When(x => !x.TaskId.HasValue)
            .WithMessage("Either Task ID or Metric Type ID must be provided");

        RuleFor(x => x.ScheduleType)
            .IsInEnum().WithMessage("Invalid schedule type");

        RuleFor(x => x.Schedule)
            .NotEmpty().WithMessage("Schedule is required")
            .MaximumLength(500).WithMessage("Schedule must not exceed 500 characters");
    }
}

public class UpdateReminderCommandValidator : AbstractValidator<UpdateReminderCommand>
{
    public UpdateReminderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reminder ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(1).WithMessage("Title must be at least 1 character")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.ScheduleType)
            .IsInEnum().WithMessage("Invalid schedule type");

        RuleFor(x => x.Schedule)
            .NotEmpty().WithMessage("Schedule is required")
            .MaximumLength(500).WithMessage("Schedule must not exceed 500 characters");
    }
}

public class DeleteReminderCommandValidator : AbstractValidator<DeleteReminderCommand>
{
    public DeleteReminderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reminder ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}