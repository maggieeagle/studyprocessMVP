using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoleRepository
    {
        Role? GetByName(string name);
        IReadOnlyList<Role> GetAll();
    }
}