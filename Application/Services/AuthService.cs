using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Email y contraseña son obligatorios.");

        var user = await _userRepository.GetUserByEmailAsync(dto.Email);

        // Mismo mensaje para "no existe" y "contraseña incorrecta":
        // así no se puede averiguar qué emails están registrados.
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email o contraseña incorrectos.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("El usuario está inactivo.");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt,
            User = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? string.Empty
            }
        };
    }
}