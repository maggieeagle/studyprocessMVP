using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure;


namespace Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<Student> GetById(int id)
        {
            return _context.Students
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public Task<List<int>> GetEnrolledCourseIds(int studentId)
        {
            return _context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Select(e => e.CourseId)
                .ToListAsync();
        }
        public async Task Save(Student student)
        {
            if (student.Id == 0)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return;
            }

            var entry = _context.Entry(student);

            if (entry.State == EntityState.Detached)
            {
                var existingStudent = await _context.Students
                    .Include(s => s.Enrollments)
                    .FirstOrDefaultAsync(s => s.Id == student.Id);

                if (existingStudent != null)
                {
                    _context.Entry(existingStudent).CurrentValues.SetValues(student);

                    existingStudent.Enrollments.Clear();
                    foreach (var enrollment in student.Enrollments)
                    {
                        existingStudent.Enrollments.Add(enrollment);
                    }
                }
                else
                {
                    _context.Students.Add(student);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}