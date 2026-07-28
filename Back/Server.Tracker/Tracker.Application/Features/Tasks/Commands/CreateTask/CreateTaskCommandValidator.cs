using FluentValidation;

namespace Tracker.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Заголовок задачи не может быть пустым.")
                .MaximumLength(200).WithMessage("Заголовок не должен превышать 200 символов.");

            RuleFor(v => v.NodeId)
                .NotEmpty().WithMessage("Задача должна быть привязана к подгруппе (Node).");

            RuleFor(v => v.DueDate)
                .Must(date => !date.HasValue || date.Value > DateTime.UtcNow)
                .WithMessage("Срок выполнения задачи не может быть в прошлом.");
        }
    }
}
