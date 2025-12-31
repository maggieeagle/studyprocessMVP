using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<StudentCourseService> _logger;

        public StudentCourseService(ICourseRepository courseRepo, IStudentRepository studentRepo, ILogger<StudentCourseService> logger)
        {
            _courses = courseRepo;
            _students = studentRepo;
            _logger = logger;
        }

        public async Task<List<StudentCourseDTO>> GetAllCoursesWithEnrollmentStatusAsync(int userId, string? searchText, string? courseCode, CourseStatus? statusFilter, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = new List<StudentCourseDTO>();

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
                {
                    _logger.LogWarning("Student profile not found for UserId {UserId}", userId);

                    throw new Exception("Student profile not found for this user.");
                }

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
                        .Where(c => c.EndDate <= endDate.Value)
                        .ToList();

                foreach (var course in allCourses)
                {
                    var isEnrolled = enrolledCourseIds.Contains(course.Id);

                    // calculate displayed status based on enrollment and dates
                    // available - not enrolled and end date in future
                    // passed - not enrolled and end date in past
                    // enrolled - enrolled and end date in future
                    // completed - enrolled and end date in past
                    Course.CourseStatus displayedStatus;
                    if (!isEnrolled && course.EndDate >= DateTime.Today)
                        displayedStatus = Course.CourseStatus.Available;
                    else if(!isEnrolled && course.EndDate < DateTime.Today)
                        displayedStatus = Course.CourseStatus.Passed;
                    else if (isEnrolled && course.EndDate < DateTime.Today)
                        displayedStatus = Course.CourseStatus.Completed;
                    else if (isEnrolled)
                        displayedStatus = Course.CourseStatus.Enrolled;
                    else
                        //fallback
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading courses for UserId {UserId}", userId);

                throw;
            }
        }

        public async Task Enroll(int studentId, int courseId)
        {
            try
            {
                _logger.LogInformation("Enrollment attempt: StudentId {StudentId}, CourseId {CourseId}", studentId, courseId);

                var student = await _students.GetById(studentId);
                if (student == null)
                {
                    _logger.LogWarning("Enrollment failed: Student not found for StudentId {StudentId}", studentId);

                    throw new Exception("Student not found");
                }

                var course = await _courses.GetById(courseId);
                if (course == null)
                {
                    _logger.LogWarning("Enrollment failed: Course not found for CourseId {CourseId}", courseId);
                    throw new Exception("Course not found");
                }

                if (course.EndDate < DateTime.Today)
                {
                    _logger.LogWarning("Enrollment failed: Course {CourseId} already ended", courseId);
                    throw new InvalidOperationException("Cannot enroll in a finished course.");
                }


                // Use domain method
                if (!student.Enrollments.Any(e => e.CourseId == courseId))
                {
                    student.EnrollInCourse(course);
                    await _students.Save(student);  // Save changes via repository

                    _logger.LogInformation("StudentId {StudentId} successfully enrolled in CourseId {CourseId}", studentId, courseId);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during enrollment: StudentId {StudentId}, CourseId {CourseId}", studentId, courseId);

                throw;
            }
        }
    }
}
