using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrueCode.UserService.Users;

namespace TrueCode.UserService.Authentication;

public class AccessTokenCreator : IAccessTokenCreator<UserDb>
{
    private readonly IOptionsMonitor<UserTokenCreationOptions> _optionsMonitor;

    public AccessTokenCreator(IOptionsMonitor<UserTokenCreationOptions> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public string CreateToken(UserDb user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, "getcurrency"),
            new Claim(ClaimTypes.Role, "setcurrency"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Уникальный ID токена
        };

        var options = _optionsMonitor.CurrentValue;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(options.ExpiredIn),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}