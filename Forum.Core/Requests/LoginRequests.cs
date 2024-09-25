using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Forum.Core.Requests;

public class LoginRequests
{
    public required string Username { get; set; }

    [PasswordPropertyText]
    public required string Password { get; set; }
}
