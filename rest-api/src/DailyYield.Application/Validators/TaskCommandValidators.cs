using DailyYield.Application.Commands;
using FluentValidation;

namespace DailyYield.Application.Validators;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(1).WithMessage("Title must be at least 1 character")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
    }
}

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(1).WithMessage("Title must be at least 1 character")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
    }
}

public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}

public class StartTaskTimerCommandValidator : AbstractValidator<StartTaskTimerCommand>
{
    public StartTaskTimerCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}

public class StopTaskTimerCommandValidator : AbstractValidator<StopTaskTimerCommand>
{
    public StopTaskTimerCommandValidator()
    {
        RuleFor(x => x.TimerId)
            .NotEmpty().WithMessage("Timer ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}

public class AddTaskCollaboratorCommandValidator : AbstractValidator<AddTaskCollaboratorCommand>
{
    public AddTaskCollaboratorCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.CollaboratorUserId)
            .NotEmpty().WithMessage("Collaborator user ID is required");

        RuleFor(x => x.OwnerUserId)
            .NotEmpty().WithMessage("Owner user ID is required");

        RuleFor(x => x)
            .Must(x => x.CollaboratorUserId != x.OwnerUserId)
            .WithMessage("Collaborator user ID cannot be the same as owner user ID");
    }
}

public class RemoveTaskCollaboratorCommandValidator : AbstractValidator<RemoveTaskCollaboratorCommand>
{
    public RemoveTaskCollaboratorCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("Task ID is required");

        RuleFor(x => x.CollaboratorUserId)
            .NotEmpty().WithMessage("Collaborator user ID is required");

        RuleFor(x => x.OwnerUserId)
            .NotEmpty().WithMessage("Owner user ID is required");
    }
}