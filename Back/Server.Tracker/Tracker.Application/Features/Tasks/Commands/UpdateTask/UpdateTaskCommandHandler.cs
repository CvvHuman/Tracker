using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tracker.Application.Abstractions;
using Tracker.Application.DTOs;

namespace Tracker.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler: IRequestHandler<UpdateTaskCommand,TodoTaskDTO> //есть смысл при улучшении проекта разделить Update на UpdateTitle, UpdateIsCompleted и UpdateDueDate
    {
        private readonly ITrackerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTaskCommandHandler(ITrackerDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TodoTaskDTO> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var taskUpdate = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == userId, cancellationToken);

            if (taskUpdate == null)
                throw new UnauthorizedAccessException("Task is not found");

            //taskUpdate.Title = request.Title;
            //taskUpdate.DueDate = request.DueDate;
            //taskUpdate.IsCompleted = request.IsCompleted;
            request.Adapt(taskUpdate);//или сделать update в сущности

            await _context.SaveChangesAsync(cancellationToken);

            return taskUpdate.Adapt<TodoTaskDTO>();
        }
    }
}
