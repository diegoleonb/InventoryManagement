using InventoryAPI.DTOs;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventoryAPI.Services
{
  public class AuthService : IAuthService
  {
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration configuration)
    {
      _userRepository = userRepository;
      _roleRepository = roleRepository;
      _configuration = configuration;
    }

    public async Task<UserResponseDto> Register(UserRegisterDto registerDto)
    {
      if (await _userRepository.UserExists(registerDto.Username, registerDto.Email))
        throw new ArgumentException("Username or email already exists");

      // Verify role exists
      var role = await _roleRepository.GetByIdAsync(registerDto.RoleId);
      if (role == null)
        throw new ArgumentException("Invalid role");

      CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

      var user = new User
      {
        Username = registerDto.Username,
        Email = registerDto.Email,
        PasswordHash = passwordHash,
        PasswordSalt = passwordSalt,
        RoleId = registerDto.RoleId
      };

      await _userRepository.AddAsync(user);
      await _userRepository.SaveAllAsync();

      // Load role for response
      user.Role = role;

      var token = GenerateJwtToken(user);

      return new UserResponseDto
      {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        RoleId = user.RoleId,
        RoleName = user.Role.Name,
        Token = token
      };
    }

    public async Task<UserResponseDto> Login(UserLoginDto loginDto)
    {
      var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
      if (user == null)
        throw new UnauthorizedAccessException("Invalid credentials");

      if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
        throw new UnauthorizedAccessException("Invalid credentials");

      var token = GenerateJwtToken(user);

      return new UserResponseDto
      {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        RoleId = user.RoleId,
        RoleName = user.Role.Name,
        Token = token
      };
    }

    public string GenerateJwtToken(User user)
    {
      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8
          .GetBytes(_configuration["Jwt:Secret"]));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(7),
        SigningCredentials = creds,
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"]
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    public async Task<bool> UserExists(string username, string email)
    {
      return await _userRepository.UserExists(username, email);
    }

    public int? GetUserIdFromToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

      try
      {
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidIssuer = _configuration["Jwt:Issuer"],
          ValidAudience = _configuration["Jwt:Audience"],
          ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        return userId;
      }
      catch
      {
        return null;
      }
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512(passwordSalt))
      {
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
      }
    }
  }
}