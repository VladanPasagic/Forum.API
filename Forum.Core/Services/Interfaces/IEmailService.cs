using Microsoft.AspNetCore.Identity.UI.Services;

namespace Forum.Core.Services.Interfaces;

public interface IEmailService
{
    public Task SendEmail(string receiver, string subject, string body);
}
