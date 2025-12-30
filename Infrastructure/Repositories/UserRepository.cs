using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public User? GetByEmailAndPassword(string email, string password)
        {
            return _context.Users
                .AsEnumerable()
                .FirstOrDefault(u =>
                    u.Email.Value == email &&
                    u.Password == password);
        }
        public User? GetByEmail(string email)
        {
            return _context.Users
                .AsEnumerable()
                .FirstOrDefault(u =>
                    u.Email.Value == email);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
