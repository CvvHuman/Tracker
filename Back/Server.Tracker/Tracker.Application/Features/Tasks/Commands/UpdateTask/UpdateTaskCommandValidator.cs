using FluentValidation;

namespace Tracker.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator: AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Заголовок задачи не может быть пустым.")
                .MaximumLength(200).WithMessage("Заголовок не должен превышать 200 символов.");
            RuleFor(d => d.DueDate)
                .Must(date => !date.HasValue || date.Value > DateTime.UtcNow)
                .WithMessage("Срок выполнения задачи не может быть в прошлом.");
        }
    }
}
