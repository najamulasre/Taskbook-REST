using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.WebApi.Parameters.GroupMemberships
{
    public class CreateGroupMembershipParameters
    {
        [Required]
        public string UserName { get; set; }
    }
}
