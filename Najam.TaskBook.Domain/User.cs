using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Najam.TaskBook.Domain
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

        public virtual ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
    }
}