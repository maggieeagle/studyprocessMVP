using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Domain.Entities.Course;

namespace Application.DTO
{
    public enum CourseStatusDTO
    {
        Available,
        Enrolled,
        Completed,
        Passed
    }
    public class StudentCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsEnrolled { get; set; }
        public Course.CourseStatus DisplayedStatus { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Enrollment button available only if not enrolled yet
        public bool CanEnroll => !IsEnrolled &&
        DisplayedStatus == Course.CourseStatus.Available &&
        DateTime.Now <= EndDate;
    }
}
