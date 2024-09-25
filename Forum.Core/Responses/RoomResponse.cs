namespace Forum.Core.Responses;

public class RoomResponse
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Author { get; set; }

    public string UserId { get; set; }
}
