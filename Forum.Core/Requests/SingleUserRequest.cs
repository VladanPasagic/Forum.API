namespace Forum.Core.Requests;

public class SingleUserRequest
{
    public string Role { get; set; }

    public List<PermissionRequest> Permissions { get; set; }
}
