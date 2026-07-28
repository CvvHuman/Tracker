using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Tracker.Application.Abstractions;

namespace Tracker.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITrackerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTaskCommandHandler(ITrackerDbContext context, ICurrentUserService currentUserService )
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var taskDel = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == request.TaskId && t.UserId == userId, cancellationToken);

            if (taskDel == null)
                throw new UnauthorizedAccessException("Task is not found");

            _context.TodoTasks.Remove(taskDel);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
