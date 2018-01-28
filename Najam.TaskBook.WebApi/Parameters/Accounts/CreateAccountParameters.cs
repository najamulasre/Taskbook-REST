using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.WebApi.Parameters.Accounts
{
    public class CreateAccountParameters : LogonParameters
    {
        [Required]
        [EmailAddress(ErrorMessage = "{0} is not a valid email address format")]
        [StringLength(64, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
