namespace Models;

public class PermissionGroup : BaseEntity
{
    public string Name { get; set; }
    public string? Permissions { get; set; }
    public List<User> Users { get; set; }
}