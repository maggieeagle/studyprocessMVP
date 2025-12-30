using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Entities.Course;

namespace Application.Services
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly ICourseRepository _courses;
        private readonly IStudentRepository _students;

        public StudentCourseService(ICourseRepository courseRepo, IStudentRepository studentRepo)
        {
            _courses = courseRepo;
            _students = studentRepo;
        }

        public async Task<List<StudentCourseDTO>> GetAllCoursesWithEnrollmentStatusAsync(int userId, string? searchText, string? courseCode, CourseStatus? statusFilter, DateTime? startDate, DateTime? endDate)
        {
            var result = new List<StudentCourseDTO>();

            // TODO: change for SQL query?
            var allCourses = await _courses.GetAllAsync();

            bool isTeacher = await _courses.IsTeacherAsync(userId);

            if (isTeacher)
            {
                foreach (var course in allCourses)
                {
                    result.Add(new StudentCourseDTO
                    {
                        CourseId = course.Id,
                        CourseName = course.Name,
                        Code = course.Code,
                        IsEnrolled = false,
                        StartDate = course.StartDate,
                        EndDate = course.EndDate
                    });
                }
                return result;
            }

            var student = await _students.GetById(userId);
            if (student == null)
                throw new Exception("Student profile not found for this user.");

            var enrolledCourseIds = new HashSet<int>(student.Enrollments.Select(e => e.CourseId));

            if (!string.IsNullOrWhiteSpace(searchText))
                allCourses = allCourses
                    .Where(c => c.Name.Contains(searchText))
                    .ToList();

            if (!string.IsNullOrWhiteSpace(courseCode))
                allCourses = allCourses
                    .Where(c => c.Code.Contains(courseCode))
                    .ToList();

           // if (statusFilter.HasValue)
               // allCourses = allCourses
                  //  .Where(c => c.Status == statusFilter.Value)
                  //  .ToList();

            if (startDate.HasValue)
                allCourses = allCourses
                    .Where(c => c.StartDate >= startDate.Value)
                    .ToList();

            if (endDate.HasValue)
                allCourses = allCourses
                    .Where(c => c.StartDate <= endDate.Value)
                    .ToList();

            foreach (var course in allCourses)
            {
                var isEnrolled = enrolledCourseIds.Contains(course.Id);

                // calculate displaye status based on enrollment and dates
                // available - not enrolled and end date in future
                // enrolled - enrolled and end date in future
                // completed - enrolled and end date in past
                Course.CourseStatus displayedStatus;
                if (!isEnrolled && course.EndDate >= DateTime.Today)
                    displayedStatus = Course.CourseStatus.Available;
                    else if (isEnrolled && course.EndDate < DateTime.Today)
                    displayedStatus = Course.CourseStatus.Completed;
                    else if (isEnrolled)
                    displayedStatus = Course.CourseStatus.Enrolled;
                    else
                    displayedStatus = Course.CourseStatus.Available;

                if (statusFilter.HasValue && displayedStatus != statusFilter.Value)
                    continue;

                result.Add(new StudentCourseDTO
                {
                    CourseId = course.Id,
                    CourseName = course.Name,
                    Code = course.Code,
                    IsEnrolled = isEnrolled,
                    DisplayedStatus = displayedStatus,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate
                });
            }

            return result;
        }

        public async Task Enroll(int studentId, int courseId)
        {
            var student = await _students.GetById(studentId);
            if (student == null)
                throw new Exception("Student not found");

            var course = await _courses.GetById(courseId);
            if (course == null)
                throw new Exception("Course not found");

            // Use domain method
            if (!student.Enrollments.Any(e => e.CourseId == courseId))
            {
                student.EnrollInCourse(course);
                await _students.Save(student);  // Save changes via repository
            }
        }
    }
}
