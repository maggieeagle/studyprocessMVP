namespace Domain.Entities
{
    public class ExamAssignment : Assignment
    {
        public DateTime ExamDate { get; private set; }

        protected ExamAssignment() : base() { }  // for EF core

        public ExamAssignment(string title, string description, DateTime dueDate, Course course, DateTime examDate) : base(title, description, dueDate, course)
        {
            ExamDate = examDate;
        }
    }
}
