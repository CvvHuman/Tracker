using MediatR;
using Tracker.Application.Abstractions;
using Tracker.Domain.Entities;

namespace Tracker.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITrackerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateTaskCommandHandler(ITrackerDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var todoTask = new TodoTask
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                DueDate = request.DueDate,
                NodeId = request.NodeId,
                UserId = userId!.Value
            };

            await _context.TodoTasks.AddAsync(todoTask,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return todoTask.Id;
        }
    }
}
