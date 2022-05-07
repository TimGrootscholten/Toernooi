using System.Collections.ObjectModel;

namespace Models
{
    public class User : BaseEntity
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual List<PermissionGroup> PermissionGroups { get; set; }
    }
}
