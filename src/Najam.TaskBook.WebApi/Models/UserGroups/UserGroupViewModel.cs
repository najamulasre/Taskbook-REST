using System;

namespace Najam.TaskBook.WebApi.Models.UserGroups
{
    public class UserGroupViewModel
    {
        public Guid GroupId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public string RelationType { get; set; }
    }
}
