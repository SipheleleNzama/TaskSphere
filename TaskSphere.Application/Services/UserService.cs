using AutoMapper;
using Org.BouncyCastle.Crypto.Generators;
using TaskSphere.Application.DTOs;
using TaskSphere.Domain.Entities;
using TaskSphere.Domain.Interfaces;

namespace TaskSphere.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        if (await _userRepository.ExistsAsync(dto.Username, dto.Email))
            throw new InvalidOperationException("Username or email already exists");

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.CreatedAt = DateTime.UtcNow;

        var created = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserDto>(created);
    }
}