using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public UserService(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(user.LastName))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("El email es obligatorio.");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new ArgumentException(
                "La contraseña debe tener al menos 8 caracteres.");

        var existing = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existing != null)
            throw new InvalidOperationException(
                "Ya existe un usuario con ese email.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user.IsActive = true;
        user.RoleId = (int)RoleType.Common;

        await _userRepository.AddUserAsync(user);

        return await _userRepository.GetUserByIdAsync(user.Id) ?? user;
    }

    public async Task UpdateUserAsync(User user, int actingUserId)
    {
        var existing = await _userRepository.GetUserByIdAsync(user.Id);

        if (existing == null)
            throw new KeyNotFoundException(
                $"No se encontró el usuario con ID {user.Id}.");

        await EnsureCanManageAsync(actingUserId, existing);

        if (string.IsNullOrWhiteSpace(user.FirstName) ||
            string.IsNullOrWhiteSpace(user.LastName))
            throw new ArgumentException("Nombre y apellido son obligatorios.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("El email es obligatorio.");

        var emailOwner = await _userRepository.GetUserByEmailAsync(user.Email);
        if (emailOwner != null && emailOwner.Id != user.Id)
            throw new InvalidOperationException(
                "Ya existe otro usuario con ese email.");

        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.Email = user.Email;

        await _userRepository.UpdateUserAsync(existing);
    }

    public async Task DeactivateUserAsync(int id, int actingUserId)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException(
                $"No se encontró el usuario con ID {id}.");

        if (actingUserId == id)
            throw new InvalidOperationException("No podés desactivar tu propia cuenta.");

        await EnsureCanManageAsync(actingUserId, user);

        if (!user.IsActive)
            throw new InvalidOperationException("El usuario ya está inactivo.");

        user.IsActive = false;

        await _userRepository.UpdateUserAsync(user);
    }

    // SuperAdmin: a cualquiera. Admin: a Common y a sí mismo. Common: solo a sí mismo.
    private async Task EnsureCanManageAsync(int actingUserId, User target)
    {
        var acting = await _userRepository.GetUserByIdAsync(actingUserId)
            ?? throw new KeyNotFoundException("Usuario que realiza la acción no encontrado.");

        if (acting.RoleId == (int)RoleType.SuperAdmin)
            return;

        if (acting.Id == target.Id)
            return;

        if (acting.RoleId == (int)RoleType.Admin && target.RoleId != (int)RoleType.SuperAdmin)
            return;

        throw new UnauthorizedAccessException(
            "No tenés permisos para modificar a este usuario.");
    }
    public async Task UpdateUserRoleAsync(int targetUserId, int newRoleId, int actingUserId)
    {
        if (actingUserId == targetUserId)
            throw new InvalidOperationException("No podés cambiar tu propio rol.");

        var acting = await _userRepository.GetUserByIdAsync(actingUserId)
            ?? throw new KeyNotFoundException("Usuario que realiza la acción no encontrado.");

        if (acting.RoleId != (int)RoleType.SuperAdmin)
            throw new UnauthorizedAccessException("Solo un Super Admin puede cambiar roles.");

        var target = await _userRepository.GetUserByIdAsync(targetUserId)
            ?? throw new KeyNotFoundException($"No se encontró el usuario con ID {targetUserId}.");

        var role = await _userRoleRepository.GetUserRoleByIdAsync(newRoleId)
            ?? throw new KeyNotFoundException($"No existe el rol con ID {newRoleId}.");

        if (!role.IsActive)
            throw new InvalidOperationException("El rol seleccionado no está activo.");

        target.RoleId = role.Id;
        target.Role = null; // evita que EF intente reasignar el rol anterior cargado

        await _userRepository.UpdateUserAsync(target);
    }
}