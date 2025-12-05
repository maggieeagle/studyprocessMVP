namespace ApplicationLayer.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudents();
        Task<Student?> GetById(int id);
        Task Add(Student student);
        Task Update(Student student);
        Task Delete(int id);
    }
}
