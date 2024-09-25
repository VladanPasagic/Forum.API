namespace Forum.Core.Requests;

public class RoomRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public int CategoryId { get; set; }

    public string Content { get; set; }
}
