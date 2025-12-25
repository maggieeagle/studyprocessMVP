using Application.Interfaces;
using Domain.Entities;
using Application.DTO;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.AsNoTracking().ToListAsync();
        }
        public Task<Course?> GetById(int id)
        {
            return _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetByIdWithAssignmentsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Assignments)   // include assignments
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<CourseDetailDTO?> GetCourseDetailsAsync(int courseId, int userId)
        {
            var isTeacher = await IsTeacherAsync(userId);

            int? studentId = null;
            if (!isTeacher)
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
                studentId = student?.Id;
            }

            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Assignments)
                .Where(c => c.Id == courseId)
                .Select(c => new CourseDetailDTO
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    Code = c.Code,
                    TeacherName = c.TeacherName,
                    Assignments = c.Assignments.Select(a => new AssignmentDTO
                    {
                        Id = a.Id,
                        Name = a.Title,
                        Type = a is HomeworkAssignment ? "Homework" :
                               a is ExamAssignment ? "Exam" : "General",
                        DueDate = a.DueDate,
                        Status = isTeacher ? "Active" :
                                 (_context.Submissions.Any(s => s.AssignmentId == a.Id && s.StudentId == studentId)
                                 ? "Submitted"
                                 : (a.DueDate < DateTime.Now ? "Overdue" : "Not Submitted")),
                        Grade = _context.Submissions
                            .Where(s => s.AssignmentId == a.Id && s.StudentId == studentId)
                            .Select(s => s.Grade)
                            .FirstOrDefault()

                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CourseDetailDTO>> GetAllCoursesAsync()
        {
            return await _context.Courses
               .AsNoTracking()
               .Select(c => new CourseDetailDTO
               {
                   CourseId = c.Id,
                   CourseName = c.Name,
                   Code = c.Code,
                   TeacherName = c.TeacherName
               }).ToListAsync();
        }

        public async Task SubmitAssignmentAsync(int userId, int assignmentId, string content)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) throw new Exception("Only students can submit assignments.");

            var submission = new Submission
            {
                StudentId = student.Id,
                AssignmentId = assignmentId,
                Content = content,
                SubmittedAt = DateTime.Now,
                Student = null!,
                Assignment = null!
            };

            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();
        }

        public async Task AddAssignmentAsync(int courseId, AddAssignmentDTO dto)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) throw new Exception("Course not found");

            Assignment assignment = dto.Type switch
            {
                "Homework" => new HomeworkAssignment(dto.Title, dto.Description, dto.DueDate, course, dto.MaxPoints),
                _ => new ExamAssignment(dto.Title, dto.Description, dto.DueDate, course, dto.ExamDate ?? dto.DueDate)
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTeacherAsync(int userId)
        {
            return await _context.Teachers.AnyAsync(t => t.UserId == userId);
        }
        public async Task<List<SubmissionDTO>> GetSubmissionsForAssignmentAsync(int assignmentId)
        {
            return await _context.Submissions
                .AsNoTracking()
                .Include(s => s.Student)
                .Where(s => s.AssignmentId == assignmentId)
                .Select(s => new SubmissionDTO
                {
                    SubmissionId = s.Id,
                    StudentId = s.StudentId,
                    StudentFirstName = s.Student.FirstName,
                    StudentLastName = s.Student.LastName,
                    Answer = s.Content,
                    SubmittedAt = s.SubmittedAt,
                    Grade = s.Grade
                })
                .ToListAsync();
        }

        public async Task SaveGradesAsync(List<SubmissionDTO> submissions)
        {
            var submissionIds = submissions
                .Where(s => s.Grade.HasValue)
                .Select(s => s.SubmissionId)
                .ToList();

            if (!submissionIds.Any())
                return;

            var entities = await _context.Submissions
                .Where(s => submissionIds.Contains(s.Id))
                .ToListAsync();

            foreach (var entity in entities)
            {
                var dto = submissions.First(s => s.SubmissionId == entity.Id);
                entity.Grade = dto.Grade;
            }

            await _context.SaveChangesAsync();
        }

    }
}