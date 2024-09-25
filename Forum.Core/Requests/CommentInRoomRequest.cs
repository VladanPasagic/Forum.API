using Forum.EF.Entities;

namespace Forum.Core.Requests;

public class CommentInRoomRequest
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int ChannelId { get; set; }
}
