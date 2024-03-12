using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace SolarWatch.Services.Authentication;

public class TokenService : ITokenService
{
    private const int ExpirationMinutes = 30;
    private readonly string _validIssuer;
    private readonly string _validAudience;
    private readonly string _issuerSigningKey;

    public TokenService(string validIssuer, string validAudience, string issuerSigningKey)
    {
        _validIssuer = validIssuer;
        _validAudience = validAudience;
        _issuerSigningKey = issuerSigningKey;
    }

    public string CreateToken(IdentityUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

        var token = CreateJwtToken(CreateClaims(user), CreateSigningCredentials(), expiration);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
    {
        return new JwtSecurityToken(_validIssuer, _validAudience, claims, expires: expiration,
            signingCredentials: credentials);
    }

    private List<Claim> CreateClaims(IdentityUser user)
    {
        try
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerSigningKey)),
            SecurityAlgorithms.HmacSha256);
    }
}