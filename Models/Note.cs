namespace BlogNotes.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
