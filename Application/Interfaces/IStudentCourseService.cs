using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IStudentCourseService
    {
        Task<List<DTO.StudentCourseDTO>> GetAllCoursesWithEnrollmentStatusAsync(int studentId);
        Task Enroll(int studentId, int courseId);
    }
}
