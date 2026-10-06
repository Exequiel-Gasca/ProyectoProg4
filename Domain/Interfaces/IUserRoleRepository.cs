using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRoleRepository
{
    Task<IEnumerable<UserRole>> GetAllUserRoleAsync();
    Task<UserRole?> GetUserRoleByIdAsync(int id);
    Task AddUserRoleAsync(UserRole userRole);
    Task UpdateUserRoleAsync(UserRole userRole);
    Task DeleteUserRoleAsync(int id);
}