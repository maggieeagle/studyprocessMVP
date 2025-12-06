using Domain.Common;

namespace Domain.Entities
{
    public class Grade : BaseEntity
    {
        public int StudentId { get; private set; }
        public Student Student { get; private set; }
        public int AssignmentId { get; private set; }
        public Assignment Assignment { get; private set; }
        public decimal Score { get; private set; }

        public Grade(Student student, Assignment assignment, decimal score)
        {
            Student = student ?? throw new ArgumentNullException(nameof(student));
            Assignment = assignment ?? throw new ArgumentNullException(nameof(assignment));
            if (score < 0) throw new DomainException("Score cannot be negative.");
            StudentId = student.Id;
            AssignmentId = assignment.Id;
            Score = score;
        }

        public void UpdateScore(decimal newScore)
        {
            if (newScore < 0) throw new DomainException("Score cannot be negative.");
            Score = newScore;
        }
    }
}
