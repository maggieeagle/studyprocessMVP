using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoleRepository(AppDbContext context) : IRoleRepository
    {
        private readonly AppDbContext _context = context;

        public Role? GetByName(string name)
        {
            return _context.Roles
                .FirstOrDefault(r => r.Name == name);
        }

        public IReadOnlyList<Role> GetAll()
        {
            return [.. _context.Roles.OrderBy(r => r.Name).AsNoTracking()];
        }
    }
}
