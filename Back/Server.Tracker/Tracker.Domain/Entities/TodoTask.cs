using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Domain.Entities
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

 
        public Guid NodeId { get; set; } 
        public Node Node { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
