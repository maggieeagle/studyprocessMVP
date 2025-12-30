using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        User? GetByEmailAndPassword(string email, string password);
        User? GetByEmail(string email);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}