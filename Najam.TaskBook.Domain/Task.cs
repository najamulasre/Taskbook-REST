using System;

namespace Najam.TaskBook.Domain
{
    public class Task
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public Guid GroupId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime? DateCompleted { get; set; }

        public virtual User User { get; set; }

        public virtual Group Group { get; set; }
    }
}
