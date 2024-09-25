namespace Forum.Core.Responses;

public class SingleUserResponse : BaseResponse
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public List<PermissionResponse> Permissions { get; set; } = [];
}
