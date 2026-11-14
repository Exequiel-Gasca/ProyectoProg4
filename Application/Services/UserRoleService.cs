using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;

    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }

    public async Task<IEnumerable<UserRole>> GetAllUserRoleAsync()
    {
        return await _userRoleRepository.GetAllUserRoleAsync();
    }

    public async Task<UserRole?> GetUserRoleByIdAsync(int id)
    {
        return await _userRoleRepository.GetUserRoleByIdAsync(id);
    }
}