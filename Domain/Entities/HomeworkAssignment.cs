namespace App.Domain
	public class HomeworkAssignment : Assignment
	{
		public int MaxPoints { get; private set; }

		public HomeworkAssignment(string title, DateTime dueDate, Course course, int maxPoints)
			: base(title, dueDate, course)
		{
			MaxPoints = maxPoints;
		}
	}
}
