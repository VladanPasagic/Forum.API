using Forum.Core.Requests;
using Forum.Core.Responses;

namespace Forum.Core.Services.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponse> Login(LoginRequests request);
    Task<RegisterResponse> Register(RegisterRequests request);
    Task<bool> TwoFactorAuthRequest(string id, TwoFactorRequests request);
    Task Logout(string id);
}
