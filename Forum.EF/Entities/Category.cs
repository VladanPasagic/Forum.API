namespace Forum.EF.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Room> Rooms { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; }
}
