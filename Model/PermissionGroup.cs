namespace Models;

public class PermissionGroup : BaseEntity
{
    public virtual string Name { get; set; }
    public virtual string? Permissions { get; set; }
    public virtual List<User> Users { get; set; }
}