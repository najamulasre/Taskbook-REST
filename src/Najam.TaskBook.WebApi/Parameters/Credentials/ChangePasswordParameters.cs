using System.ComponentModel.DataAnnotations;
using Najam.TaskBook.WebApi.Attributes;

namespace Najam.TaskBook.WebApi.Parameters.Credentials
{
    public class ChangePasswordParameters
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [Password]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
