using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Application.DTOs
{
    public record TodoTaskDTO(
        Guid Id,
        string Title,
        bool IsCompleted,
        DateTime CreatedAt,
        DateTime? DueDate
    );
}