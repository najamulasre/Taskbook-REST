using System;

namespace Najam.TaskBook.WebApi.Models.Profiles
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
