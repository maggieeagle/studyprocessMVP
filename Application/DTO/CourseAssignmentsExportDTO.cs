using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO
{
    public class CourseAssignmentsExportDTO
    {
        public string CourseName { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string AssignmentName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
