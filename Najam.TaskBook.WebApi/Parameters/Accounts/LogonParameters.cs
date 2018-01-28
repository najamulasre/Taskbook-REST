using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.WebApi.Parameters.Accounts
{
    public class LogonParameters
    {
        [Required]
        [StringLength(32, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
