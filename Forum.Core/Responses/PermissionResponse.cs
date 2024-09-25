namespace Forum.Core.Responses;

public class PermissionResponse
{
    public int Id { get; set; }
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string RequestType { get; set; }
}
