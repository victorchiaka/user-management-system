using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UMS.Features;

namespace UMS.Contracts;

public class AuthenticationService: IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthenticationService(IUserService userService, IConfiguration configuration)
    {
        this._userService = userService;
        this._configuration = configuration;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool IsVerifiedPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    public async Task<string?> AuthenticateUser(string emailAddress, string password)
    {
        User? user = await _userService.GetUserByEmail(emailAddress);

        if (user is null || !IsVerifiedPassword(password, user.PasswordHash))
        {
            return null;
        }

        string? jwtSecurityToken = GetJwtToken(user.Id);

        if (jwtSecurityToken.IsNullOrEmpty())
        {
            return null;
        }

        return jwtSecurityToken;
    }

    private string? GetJwtToken(long userId)
    {
        return new TokenService(_configuration).CreateJwtToken(userId);
    }
}