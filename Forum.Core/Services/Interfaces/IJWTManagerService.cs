using Forum.Core.Responses;
using Forum.EF.Entities;

namespace Forum.Core.Services.Interfaces;

public interface IJWTManagerService
{
    Task<TokenResponse> Authenticate(User user);
}
