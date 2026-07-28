using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Domain.Entities
{
    public class User
    {
        public Guid Id {  get; set; }
        public string NickName { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string PasswordHash { get; set; }  = string.Empty;

        public ICollection<TodoTask> TodoTasks { get; set; } = new List<TodoTask>();
    }
}
