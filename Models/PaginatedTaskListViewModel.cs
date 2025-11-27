

using System.Collections.Generic;

namespace StudentTaskManager.Models
{
    public class PaginatedTaskListViewModel
    {
        
        public List<TaskModel> Tasks { get; set; }
        public List<Category> Categories { get; set; }

        public int PageNumber { get; set; }

        
        public int TotalPages { get; set; }

        
        public int TotalTasks { get; set; }
    }
}