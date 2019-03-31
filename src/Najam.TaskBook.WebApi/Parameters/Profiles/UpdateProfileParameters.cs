using System;
using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.WebApi.Parameters.Profiles
{
    public class UpdateProfileParameters
    {
        [Required]
        [StringLength(100, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "{0} is not a valid email address format")]
        [StringLength(64, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}
