using System;

namespace Najam.TaskBook.WebApi.Models.GroupMemberships
{
    public class GroupMembershipViewModel
    {
        public Guid GroupId { get; set; }

        public string GroupName { get; set; }

        public string UserName { get; set; }
    }
}
