
using System.Collections.Generic;

namespace StudentTaskManager.Models
{
    public class PaginatedTaskListViewModel
    {
       
        public List<Category> Categories { get; set; } = new List<Category>();

       
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalTasks { get; set; }
    }
}