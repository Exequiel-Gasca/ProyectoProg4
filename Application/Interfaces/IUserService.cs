using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user, string password);
    Task UpdateUserAsync(User user);
    Task DeactivateUserAsync(int id);
    Task UpdateUserRoleAsync(int targetUserId, int newRoleId, int actingUserId);
}