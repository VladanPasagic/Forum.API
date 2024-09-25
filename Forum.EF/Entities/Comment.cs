namespace Forum.EF.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime PublishingDateTime { get; set; }
    public string UserId { get; set; }
    public bool IsPosted { get; set; }
    public bool IsHandled { get; set; }
    public bool IsDeleted { get; set; }
    public virtual User User { get; set; }
    public int RoomId { get; set; }
    public virtual Room Room { get; set; }
}
