namespace TodoApi.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }

        // Hash ng current todo block
        public string Hash { get; set; } = string.Empty;

        // Hash ng previous todo block para linked sila
        public string PreviousHash { get; set; } = string.Empty;

    }
}
