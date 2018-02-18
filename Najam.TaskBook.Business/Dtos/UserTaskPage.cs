using System;
using Najam.TaskBook.Domain;

namespace Najam.TaskBook.Business.Dtos
{
    public class UserTaskPage
    {
       public UserTaskPage(int currentPage, int pageSize, int totalCount, Task[] tasks)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Tasks = tasks;
        }

        public int CurrentPage { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public int TotalPages { get; }

        public Task[] Tasks { get; }

        public bool HasNextPage => CurrentPage < TotalPages;

        public bool HasPreviousPage => CurrentPage > 1;
    }
}
