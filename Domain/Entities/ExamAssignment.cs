namespace Domain.Entities
{
    public class ExamAssignment : Assignment
    {
        public DateTime ExamDate { get; private set; }

        public ExamAssignment(string title, DateTime dueDate, Course course, DateTime examDate)
            : base(title, dueDate, course)
        {
            ExamDate = examDate;
        }
    }
}
