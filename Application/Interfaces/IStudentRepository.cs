using System;
using System.Collections.Generic;
using System.Text;

using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetById(int id);
        Task<Student?> GetByUserId(int userId);
        Task<List<int>> GetEnrolledCourseIds(int studentId);
        Task Save(Student student);
    }
}
