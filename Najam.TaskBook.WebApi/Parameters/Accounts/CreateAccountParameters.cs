using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Najam.TaskBook.WebApi.Parameters.Accounts
{
    public class CreateAccountParameters
    {
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        [MaxLength(256)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
