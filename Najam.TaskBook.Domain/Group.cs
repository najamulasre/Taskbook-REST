using System;
using System.Collections.Generic;

namespace Najam.TaskBook.Domain
{
    public class Group
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

        public virtual ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
    }
}
