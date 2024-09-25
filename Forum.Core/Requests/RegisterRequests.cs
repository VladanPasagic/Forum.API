using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Requests;

public class RegisterRequests
{
    public required string Username { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [PasswordPropertyText]
    public required string Password { get; set; }
}
