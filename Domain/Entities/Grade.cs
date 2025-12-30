using Domain.Common;
using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities
{
    public class Grade : BaseEntity
    {
        [Required]
        public int StudentId { get; private set; }
        public Student Student { get; private set; } = null!;

        [Required]
        public int AssignmentId { get; private set; }
        public Assignment Assignment { get; private set; } = null!;
        public decimal Score { get; private set; }

        protected Grade() { }  // for EF core
        public Grade(Student student, Assignment assignment, decimal score)
        {
            ArgumentNullException.ThrowIfNull(nameof(student));
            ArgumentNullException.ThrowIfNull(nameof(assignment));

            if (score < 0) throw new DomainException("Score cannot be negative.");

            Student = student;
            Assignment = assignment;
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
