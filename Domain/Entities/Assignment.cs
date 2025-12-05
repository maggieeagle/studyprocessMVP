namespace App.Domain
{
    public abstract class Assignment
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public DateTime DueDate { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; }

        protected Assignment(string title, DateTime dueDate, Course course)
        {
            Title = title;
            DueDate = dueDate;
            Course = course;
            CourseId = course.Id;
        }
    }
}
