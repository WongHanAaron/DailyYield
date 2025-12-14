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

        RuleFor(x => x.Message)
            .MaximumLength(1000).WithMessage("Message must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Message));

        RuleFor(x => x.Schedule)
            .NotEmpty().WithMessage("Schedule is required")
            .MaximumLength(1000).WithMessage("Schedule must not exceed 1000 characters");

        // Ensure either UserId or UserGroupId is set, but not both
        RuleFor(x => x)
            .Must(x => x.UserId != Guid.Empty || x.UserGroupId != Guid.Empty)
            .WithMessage("Either UserId or UserGroupId must be specified");

        RuleFor(x => x)
            .Must(x => !(x.UserId != Guid.Empty && x.UserGroupId != Guid.Empty))
            .WithMessage("Cannot specify both UserId and UserGroupId");

        // Ensure either TaskId or MetricTypeId is set
        RuleFor(x => x)
            .Must(x => x.TaskId != Guid.Empty || x.MetricTypeId != Guid.Empty)
            .WithMessage("Either TaskId or MetricTypeId must be specified");
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

        RuleFor(x => x.Message)
            .MaximumLength(1000).WithMessage("Message must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Message));

        RuleFor(x => x.Schedule)
            .NotEmpty().WithMessage("Schedule is required")
            .MaximumLength(1000).WithMessage("Schedule must not exceed 1000 characters");

        // Ensure either UserId or UserGroupId is set, but not both
        RuleFor(x => x)
            .Must(x => x.UserId != Guid.Empty || x.UserGroupId != Guid.Empty)
            .WithMessage("Either UserId or UserGroupId must be specified");

        RuleFor(x => x)
            .Must(x => !(x.UserId != Guid.Empty && x.UserGroupId != Guid.Empty))
            .WithMessage("Cannot specify both UserId and UserGroupId");

        // Ensure either TaskId or MetricTypeId is set
        RuleFor(x => x)
            .Must(x => x.TaskId != Guid.Empty || x.MetricTypeId != Guid.Empty)
            .WithMessage("Either TaskId or MetricTypeId must be specified");
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