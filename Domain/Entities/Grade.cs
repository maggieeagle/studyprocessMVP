namespace App.Domain
{
    public class Grade
    {
        public int Id { get; private set; }
        public int StudentId { get; private set; }
        public Student Student { get; private set; }
        public int AssignmentId { get; private set; }
        public Assignment Assignment { get; private set; }
        public decimal Score { get; private set; }

        public Grade(Student student, Assignment assignment, decimal score)
        {
            Student = student;
            StudentId = student.Id;
            Assignment = assignment;
            AssignmentId = assignment.Id;
            Score = score;
        }
    }
}
