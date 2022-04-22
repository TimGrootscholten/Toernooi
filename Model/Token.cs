namespace Models
{
    public class Token : BaseEntity
    {
        public virtual Guid ClientId { get; set; }
        public virtual Guid RefreshToken { get; set; }
        public virtual DateTime RefreshTokenExpires { get; set; }
        public virtual string Username { get; set; }
    }
}
