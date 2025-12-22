using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course> GetById(int id);
        Task<List<CourseDetailDTO>> GetCoursesForStudentAsync(int studentId);
        Task<CourseDetailDTO> GetCourseDetailsAsync(int courseId, int studentId);
    }
}
