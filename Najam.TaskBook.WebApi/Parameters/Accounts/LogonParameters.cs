using System.ComponentModel.DataAnnotations;
using Najam.TaskBook.WebApi.Attributes;

namespace Najam.TaskBook.WebApi.Parameters.Accounts
{
    public class LogonParameters 
    {
        [Required]
        [StringLength(32, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string UserName { get; set; }

        [Required]
        [Password]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
