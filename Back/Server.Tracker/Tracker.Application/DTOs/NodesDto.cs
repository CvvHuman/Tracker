using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Domain.Entities;

namespace Tracker.Application.DTOs
{
    public record NodeDt(
        string Name,
        string ColorHex
    );

    public record NodeDto(
    Guid Id,
    string Name,
    string ColorHex,
    ICollection<TodoTask>? TodoTask
    );

}
