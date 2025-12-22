namespace Application.DTO
{
    public class AssignmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Homework", "Exam"
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty; // "Submitted", "Overdue", etc.
        public string Grade { get; set; } = string.Empty; // "A", "B", etc.
    }
}
