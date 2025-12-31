using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetById(int id);
        Task<Course?> GetByIdWithAssignmentsAsync(int id);
        Task<List<CourseDetailDTO>> GetAllCoursesAsync();
        Task<CourseDetailDTO?> GetCourseDetailsAsync(int courseId, int studentId);
        Task SubmitAssignmentAsync(int studentId, int assignmentId, string content);
        Task AddAssignmentAsync(int courseId, AddAssignmentDTO dto);
        Task<bool> IsTeacherAsync(int userId);
        Task<List<SubmissionDTO>> GetSubmissionsForAssignmentAsync(int assignmentId);
        Task SaveGradesAsync(List<SubmissionDTO> submissions);
        Task DeleteAssignmentAsync(int assignmentId);
        Task UpdateAssignmentAsync(int id, AssignmentDTO dto);
    }
}
