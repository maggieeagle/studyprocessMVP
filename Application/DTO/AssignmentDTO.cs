namespace Application.DTO
{
    public class AssignmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Homework", "Exam"
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty; // "Submitted", "Overdue", etc.
        public int? Grade { get; set; }
    }

    public class AddAssignmentDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Homework" or "Exam"
        public DateTime DueDate { get; set; }

        // Specific properties
        public int MaxPoints { get; set; } // For Homework
        public DateTime? ExamDate { get; set; } // For Exam
    }
}
