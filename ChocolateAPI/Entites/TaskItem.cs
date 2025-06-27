namespace ChocolateAPI.Entites
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Data.TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;
    }
}
