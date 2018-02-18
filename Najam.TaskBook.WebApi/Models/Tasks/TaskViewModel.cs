using System;

namespace Najam.TaskBook.WebApi.Models.Tasks
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public string GroupName { get; set; }

        public TaskStatus Status => CalculateStatus();

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime? DateCompleted { get; set; }

        public bool IsOverdue { get; set; }

        public string CreatedBy { get; set; }

        public string AssignedTo { get; set; }

        public DateTime? DateTimeAssigned { get; set; }

        private TaskStatus CalculateStatus()
        {
            if (DateCompleted.HasValue)
                return TaskStatus.Completed;

            if (!string.IsNullOrWhiteSpace(AssignedTo))
                return TaskStatus.Assigned;

            return TaskStatus.Unassigned;
        }
    }
}