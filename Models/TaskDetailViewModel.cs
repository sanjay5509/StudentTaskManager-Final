

using System.Collections.Generic;

namespace StudentTaskManager.Models
{
    public class TaskDetailViewModel
    {
     
        public TaskModel Task { get; set; }

       
        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();

        
        public Subtask NewSubtask { get; set; }
    }
}