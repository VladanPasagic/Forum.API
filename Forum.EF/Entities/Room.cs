namespace Forum.EF.Entities;

public class Room
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsOpened { get; set; }

    public bool IsHandled { get; set; }

    public bool IsDeleted { get; set; }

    public int CategoryId { get; set; }

    public string UserId { get; set; }

    public virtual User User { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }
}