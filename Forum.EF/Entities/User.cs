using Microsoft.AspNetCore.Identity;

namespace Forum.EF.Entities;

public class User : IdentityUser
{
    public string Name { get; set; }

    public bool IsHandled { get; set; }

    public string? ActiveTwoFactorCode { get; set; }
}
