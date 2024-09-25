using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Forum.Core.Services;

public class JWTManagerService : IJWTManagerService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public JWTManagerService(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<TokenResponse> Authenticate(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);

        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);

        List<Claim> claims = [];
        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim("id", user.Id));
        foreach (var role in roles)
        {
            claims.Add(new Claim("role", role));
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        string policy = string.Empty;
        foreach (var claim in userClaims)
        {
            policy += $"{claim.Type}:{claim.Value} ";
        }
        claims.Add(new Claim("policy", policy));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims, "jwt"),
            Claims = claims.ToDictionary(claim => claim.Type, claim => (object)claim.Value),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.Now.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new TokenResponse { Token = token };
    }
}
