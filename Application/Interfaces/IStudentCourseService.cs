using System;
using System.Collections.Generic;
using System.Text;
using static Domain.Entities.Course;

namespace Application.Interfaces
{
    public interface IStudentCourseService
    {
        Task<List<DTO.StudentCourseDTO>> GetAllCoursesWithEnrollmentStatusAsync(int studentId, string? searchText, string? courseCode, CourseStatus? statusFilter, DateTime? startDate, DateTime? endDate);
        Task Enroll(int studentId, int courseId);
    }
}
