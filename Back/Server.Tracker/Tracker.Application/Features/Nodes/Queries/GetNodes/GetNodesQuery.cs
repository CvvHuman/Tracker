using MediatR;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Application.DTOs;

namespace Tracker.Application.Features.Nodes.Quires.GetNodes
{

    public record GetNodesQuery(): IRequest<List<NodeDto>>;
}
         