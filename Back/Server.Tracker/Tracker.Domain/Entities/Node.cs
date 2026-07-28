using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Domain.Entities
{
    public class Node
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty; 

        public ICollection<TodoTask> TodoTasks { get; set; } = new List<TodoTask>();
    }
}
