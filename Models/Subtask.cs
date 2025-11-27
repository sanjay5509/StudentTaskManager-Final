namespace StudentTaskManager.Models
{
    public class Subtask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }

        public int TaskId { get; set; }
        public TaskModel Task { get; set; }
    }
}
