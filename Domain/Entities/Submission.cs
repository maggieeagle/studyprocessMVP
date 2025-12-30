using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; } = string.Empty; // The student's answer
        
        [Required]
        public DateTime SubmittedAt { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual required Student Student { get; set; }

        [ForeignKey(nameof(AssignmentId))]
        public virtual required Assignment Assignment { get; set; }

        [Range(0, 100)]
        public int? Grade { get; set; }
    }
}
