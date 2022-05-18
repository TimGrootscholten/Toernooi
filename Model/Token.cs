namespace Models;

public class Token : BaseEntity
{
    public Guid ClientId { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
    public string Username { get; set; }
}