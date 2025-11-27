using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentTaskManager.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }


        public ICollection<TaskModel> Tasks { get; set; }
    }
}
