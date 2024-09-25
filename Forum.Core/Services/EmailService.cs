using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Forum.Core.Services;

public class EmailService : IEmailService, IEmailSender<User>
{
    private IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmail(string receiver, string subject, string body)
    {
        var apiKey = _configuration["MailAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_configuration["SenderEmail"]);
        var to = new EmailAddress(receiver);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, "");
        var response = await client.SendEmailAsync(msg);
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }
}
