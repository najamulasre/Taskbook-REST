using System;
using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.WebApi.Parameters.GroupTasks
{
    public class CreateGroupTaskParameters
    {
        [Required]
        [StringLength(50, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The maximum length for {0} is {1}.")]
        public string Description { get; set; }

        [Required]
        public DateTime? Deadline { get; set; }
    }
}
