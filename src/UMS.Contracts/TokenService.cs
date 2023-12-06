using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace UMS.Contracts;

public class TokenService: ITokenService
{
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    
    public async Task<string> CreateJwtToken(long userId)
    {
        List<Claim> claims = new List<Claim>
        {
            new("sub", userId.ToString())
        };
        
        const int expiringInHours = 24;
        
        JwtSecurityToken jwt = new JwtSecurityToken(
            claims: claims,
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(expiringInHours),
            signingCredentials: GetSigningCredentials(_configuration)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private SigningCredentials GetSigningCredentials(IConfiguration configuration)
    {
        string jwtSecret = configuration["Jwt:Secret"]!;
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
    }
}