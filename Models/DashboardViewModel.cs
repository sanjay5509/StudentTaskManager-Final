using System.Collections.Generic;

namespace StudentTaskManager.Models
{
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }

   
        public List<TaskModel> RecentTasks { get; set; }
    }
}