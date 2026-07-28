  using MediatR;

namespace Tracker.Application.Features.Tasks.Commands.CreateTask
{
    public record CreateTaskCommand(
    string Title,
    DateTime? DueDate,
    Guid NodeId
    ) : IRequest<Guid>;
}
