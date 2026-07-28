using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using Tracker.Application.Abstractions;
using Tracker.Application.DTOs;

namespace Tracker.Application.Features.Nodes.Quires.GetNodes
{
    public class GetNodesQueryHandler : IRequestHandler<GetNodesQuery, List<NodeDto>>
    {
        private readonly ITrackerDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetNodesQueryHandler(ICurrentUserService currentUserService, ITrackerDbContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<List<NodeDto>> Handle(GetNodesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var userNodes = await _context.Nodes
                .Select(n => new NodeDto(
                    n.Id,
                    n.Name,
                    n.ColorHex,
                    n.TodoTasks
                        .Where(t => t.UserId == userId)
                        .ToList() 
                ))
                .ToListAsync(cancellationToken);

            return userNodes;

        }
    }
}
