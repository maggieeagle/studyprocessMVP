namespace Domain.Entities
{
    public class Submission
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty; // The student's answer
        public DateTime SubmittedAt { get; set; }
        public int StudentId { get; set; }
        public int AssignmentId { get; set; }

        public virtual required Student Student { get; set; }
        public virtual required Assignment Assignment { get; set; }
        public int? Grade { get; set; }
    }
}
