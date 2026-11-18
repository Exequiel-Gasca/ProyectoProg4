using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user, string password);
    Task UpdateUserAsync(User user, int actingUserId);
    Task DeactivateUserAsync(int id, int actingUserId);
    Task UpdateUserRoleAsync(int targetUserId, int newRoleId, int actingUserId);
}