namespace Domain.Entities
{
    public class ExamAssignment(string title, DateTime dueDate, Course course, DateTime examDate) : Assignment(title, dueDate, course)
    {
        public DateTime ExamDate { get; private set; } = examDate;
    }
}
