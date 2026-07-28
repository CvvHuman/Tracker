using MediatR;

namespace Tracker.Application.Features.Tasks.Commands.DeleteTask
{
    public record DeleteTaskCommand(Guid TaskId) : IRequest<Unit>;
}
