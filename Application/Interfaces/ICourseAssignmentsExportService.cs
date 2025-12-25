using System;
using System.Collections.Generic;
using System.Text;
using Application.DTO;

namespace Application.Interfaces
{
    public interface ICourseAssignmentsExportService
    {
        Task<List<CourseAssignmentsExportDTO>> GetAssignmentsForCourseAsync(int courseId);
        string ToCsv(List<CourseAssignmentsExportDTO> assignments);
    }
}