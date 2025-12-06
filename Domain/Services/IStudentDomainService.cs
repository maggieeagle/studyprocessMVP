namespace Domain.Services
{
    public interface IStudentDomainService
    {
        void ValidateStudentEmailUnique(string email);
    }
}
