using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentTaskManager.Models
{
    public class TaskModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Subtask> Subtasks { get; set; }
    }
}
