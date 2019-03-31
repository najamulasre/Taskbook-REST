using System;

namespace Najam.TaskBook.WebApi.Data.Entities
{
    public class UserGroup
    {
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public Guid GroupId { get; set; }

        public virtual Group Group { get; set; }

        public UserGroupRelationType RelationType { get; set; }
    }
}