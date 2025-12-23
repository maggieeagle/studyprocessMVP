using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<StudentCourseDTO>> GetAllCoursesWithEnrollmentStatusAsync(int userId)
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
                        IsEnrolled = false 
                    });
                }
                return result;
            }

            var student = await _students.GetById(userId);
            if (student == null)
                throw new Exception("Student profile not found for this user.");

            var enrolledCourseIds = new HashSet<int>(student.Enrollments.Select(e => e.CourseId));

            foreach (var course in allCourses)
            {
                result.Add(new StudentCourseDTO
                {
                    CourseId = course.Id,
                    CourseName = course.Name,
                    Code = course.Code,
                    IsEnrolled = enrolledCourseIds.Contains(course.Id)
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
