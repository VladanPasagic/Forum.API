namespace Forum.EF.Entities;

public class Permission
{
    public int Id { get; set; }
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }

    public string RequestType { get; set; }
}
