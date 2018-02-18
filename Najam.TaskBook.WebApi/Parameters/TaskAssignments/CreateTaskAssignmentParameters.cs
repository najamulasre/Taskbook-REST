using System;
using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.WebApi.Parameters.TaskAssignments
{
    public class CreateTaskAssignmentParameters
    {
        [Required]
        public Guid? TaskId { get; set; }
    }
}
