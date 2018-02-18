using System;

namespace Najam.TaskBook.Domain
{
    public class Task
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime? DateTimeCompleted { get; set; }

        public bool IsOverdue { get; set; }

        public DateTime? DateTimeAssigned { get; set; }

        public Guid? AssignedToUserId { get; set; }

        public virtual User AssignedToUser { get; set; }

        public Guid GroupId { get; set; }

        public virtual Group Group { get; set; }

        public Guid CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }

    }
}
