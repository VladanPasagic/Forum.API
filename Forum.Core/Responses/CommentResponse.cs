namespace Forum.Core.Responses;

public class CommentResponse
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime PublishingDateTime { get; set; }
    public string UserId { get; set; }
    public string Author { get; set; }

    public string RoomTitle { get; set; }

}
