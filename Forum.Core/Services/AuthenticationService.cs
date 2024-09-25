using Forum.Core.Enums;
using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Forum.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IRandomCodeGeneratorService randomCodeGeneratorService;
    private readonly IEmailService _emailService;
    private readonly IJWTManagerService _jWTManagerService;

    public AuthenticationService(UserManager<User> userManager, ILogger<AuthenticationService> logger,
        IConfiguration configuration, IRandomCodeGeneratorService randomCodeGeneratorService, IEmailService emailService, IJWTManagerService jWTManagerService)
    {
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
        this.randomCodeGeneratorService = randomCodeGeneratorService;
        _emailService = emailService;
        _jWTManagerService = jWTManagerService;
    }

    public async Task<LoginResponse> Login(LoginRequests request)
    {
        User? user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return new LoginResponse() { Message = "Invalid login information", Success = false };
        }

        if (user.IsHandled == false)
        {
            return new LoginResponse() { Message = "Account not approved yet, come back later", Success = false };
        }

        if (user.IsHandled == true && user.EmailConfirmed == false)
        {
            return new LoginResponse() { Message = "Account rejected", Success = false };
        }

        if (!(await _userManager.CheckPasswordAsync(user, request.Password)))
        {
            return new LoginResponse() { Message = "Invalid login information", Success = false };
        }

        var twoFactorCode = randomCodeGeneratorService.GenerateRandomNumericalCode(8);
        user.ActiveTwoFactorCode = twoFactorCode;
        await _userManager.UpdateAsync(user);

        await _emailService.SendEmail(user.Email!, "2FA Code", $"Your 2FA code is \n{twoFactorCode}");

        return new LoginResponse()
        {
            Success = true,
            Token = (await _jWTManagerService.Authenticate(user)).Token
        };
    }

    public Task Logout(string id)
    {
        return Task.CompletedTask;
    }

    public async Task<RegisterResponse> Register(RegisterRequests request)
    {
        User user = new() { UserName = request.Username, Email = request.Email, Name = request.Username };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("New account created! " + DateTime.UtcNow.ToString());
            if (request.Email.Equals(_configuration["AdminEmail"]))
            {
                var account = await _userManager.FindByEmailAsync(request.Email);
                if (account == null)
                {
                    return new RegisterResponse() { Success = false, Message = "Something went wrong" };
                }
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(account);
                account.IsHandled = true;
                await _userManager.AddToRoleAsync(account, Role.SuperAdmin.ToString());
                await _userManager.ConfirmEmailAsync(account, code);
                await _userManager.SetTwoFactorEnabledAsync(account, true);
                await _userManager.UpdateAsync(account);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, Role.Member.ToString());
            }
            return new RegisterResponse() { Success = true, Message = "" };
        }
        else
        {
            return new RegisterResponse() { Success = false, Message = "Something went wrong" };
        }
    }

    public async Task<bool> TwoFactorAuthRequest(string id, TwoFactorRequests request)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        if (request.Code.Equals(user.ActiveTwoFactorCode))
        {
            return true;
        }
        return false;
    }
}
