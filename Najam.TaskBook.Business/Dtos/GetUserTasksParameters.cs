using System.ComponentModel.DataAnnotations;

namespace Najam.TaskBook.Business.Dtos
{
    public class GetUserTasksParameters
    {
        private const int MaxPageSize = 100;

        [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
        public int PageNumber { get; set; } = 1;


        [Range(1, MaxPageSize, ErrorMessage = "{0} must be between {1} and {2}")]
        public int PageSize { get; set; } = 10;

        public string SearchQuery { get; set; }

        public string GroupName { get; set; }

        public bool? Overdue { get; set; }

        public string CreatedBy { get; set; }

        public string AssignedTo { get; set; }

        public string OrderBy { get; set; }
    }
}
