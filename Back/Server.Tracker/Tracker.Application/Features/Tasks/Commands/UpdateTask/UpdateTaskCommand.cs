using MediatR;
using Tracker.Application.DTOs;

namespace Tracker.Application.Features.Tasks.Commands.UpdateTask
{
    public record UpdateTaskCommand(
        Guid Id,
        string Title,
        bool IsCompleted,
        DateTime? DueDate
    ) : IRequest<TodoTaskDTO>;
}
