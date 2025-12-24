namespace Application.DTO
{
    public class CourseDetailDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public List<AssignmentDTO> Assignments { get; set; } = new();
    }
}
