using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        User? GetByEmailAndPassword(string email, string password);
        User? GetByEmail(string email);
        void Add(User user);
    }
}