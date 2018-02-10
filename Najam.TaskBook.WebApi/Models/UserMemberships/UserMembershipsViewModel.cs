using System;

namespace Najam.TaskBook.WebApi.Models.UserMemberships
{
    public class UserMembershipsViewModel
    {
        public string UserName { get; set; }

        public Guid GroupId { get; set; }

        public string GroupName { get; set; }

        public bool IsGroupActive { get; set; }

        public string MembershipType { get; set; } 
    }
}
