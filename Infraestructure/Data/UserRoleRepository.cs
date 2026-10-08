using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;

    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserRole>> GetAllUserRoleAsync()
    {
        return await _context.UserRole
            .ToListAsync();
    }

    public async Task<UserRole?> GetUserRoleByIdAsync(int id)
    {
        return await _context.UserRole
            .FirstOrDefaultAsync(ur => ur.Id == id);
    }

    public async Task AddUserRoleAsync(UserRole userRole)
    {
        await _context.UserRole.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserRoleAsync(UserRole userRole)
    {
        _context.UserRole.Update(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserRoleAsync(int id)
    {
        var userRole = await _context.UserRole.FindAsync(id);

        if (userRole != null)
        {
            _context.UserRole.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
}