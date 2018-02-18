using System;
using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.Business.Parameters
{
    public class GetUserTasksParameters
    {
        private const int MaxPageSize = 20;

        [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
        public int PageNumber { get; set; } = 1;


        [Range(1, MaxPageSize, ErrorMessage = "{0} must be between {1} and {2}")]
        public int PageSize { get; set; } = 10;
    }
}
