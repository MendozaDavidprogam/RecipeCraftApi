using RecipeCraftApi.Application.DTOs;
using RecipeCraftApi.Domain.Entities;
using RecipeCraftApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace RecipeCraftApi.Application.Services
{
public class UserService
{
private readonly IUserRepository _userRepo;
private readonly IRoleRepository _roleRepo;
private readonly IConfiguration _config;
private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepo, IRoleRepository roleRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _config = config;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserResponseDto> Register(UserRegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) ||
            string.IsNullOrWhiteSpace(dto.LastName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
            throw new Exception("All fields are required");

        var existing = await _userRepo.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new Exception("Email already registered");

        var userRole = await _roleRepo.GetByNameAsync("User") 
            ?? throw new Exception("Default role not found");

        var user = new User
        {
            Name = dto.Name,
            LastName = dto.LastName,
            Email = dto.Email,
            RoleId = userRole.Id,
            IsPublic = dto.IsPublic
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _userRepo.AddAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Role = userRole.Name,
            IsPublic = user.IsPublic,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<string> Login(UserLoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email ?? string.Empty);
        if (user == null) throw new Exception("Invalid credentials");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, dto.Password ?? string.Empty);
        if (result == PasswordVerificationResult.Failed) throw new Exception("Invalid credentials");

        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _config["JWT_SECRET"] ?? throw new Exception("JWT_SECRET not configured");
        var key = Encoding.ASCII.GetBytes(secret);
        var expiresIn = int.TryParse(_config["JWT_EXPIRES_IN"], out var seconds) ? seconds : 3600;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            }),
            Expires = DateTime.UtcNow.AddSeconds(expiresIn),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        var users = await _userRepo.GetAllAsync();
        var result = new List<UserResponseDto>();
        foreach (var u in users)
        {
            result.Add(new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role?.Name,
                IsPublic = u.IsPublic,
                CreatedAt = u.CreatedAt
            });
        }
        return result;
    }

    public async Task<UserResponseDto> GetUserById(int id)
    {
        var user = await _userRepo.GetByIdAsync(id) ?? throw new Exception("User not found");
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role?.Name,
            IsPublic = user.IsPublic,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserResponseDto> UpdateUser(int id, UserUpdateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(id) ?? throw new Exception("User not found");

        if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.LastName)) user.LastName = dto.LastName;
        if (!string.IsNullOrEmpty(dto.Password)) user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        if (dto.RoleId.HasValue)
        {
            var role = await _roleRepo.GetByIdAsync(dto.RoleId.Value);
            if (role == null) throw new KeyNotFoundException("Role not found");
            user.RoleId = role.Id;
            user.Role = role;
        }

        if (dto.IsPublic.HasValue) user.IsPublic = dto.IsPublic.Value;

        await _userRepo.UpdateAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role?.Name,
            IsPublic = user.IsPublic,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task DeleteUser(int id)
    {
        var user = await _userRepo.GetByIdAsync(id) ?? throw new Exception("User not found");
        await _userRepo.DeleteAsync(user);
    }
}

}
