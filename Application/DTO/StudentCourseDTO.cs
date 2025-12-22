using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO
{
    public class StudentCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsEnrolled { get; set; }

        // Enrollment button available only if not enrolled yet
        public bool CanEnroll => !IsEnrolled;
    }
}
