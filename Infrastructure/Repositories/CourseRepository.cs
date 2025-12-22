using Application.Interfaces;
using Domain.Entities;
using Application.DTO;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.AsNoTracking().ToListAsync();
        }
        public Task<Course?> GetById(int id)
        {
            return _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CourseDetailDTO?> GetCourseDetailsAsync(int courseId, int studentId)
        {
            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Assignments)
                .Where(c => c.Id == courseId)
                .Select(c => new CourseDetailDTO
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    Code = c.Code,
                    TeacherName = c.TeacherName,
                    Assignments = c.Assignments.Select(a => new AssignmentDTO
                    {
                        Name = a.Title,
                        Type = a is HomeworkAssignment ? "Homework" :
                               a is ExamAssignment ? "Exam" : "General",
                        DueDate = a.DueDate,
                        Status = "Not Submitted",
                        Grade = "-"
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<List<CourseDetailDTO>> GetCoursesForStudentAsync(int studentId)
        {
            return new List<CourseDetailDTO>();
        }
    }
}