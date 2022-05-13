using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class TokenDto : BaseDto
{
    public virtual Guid Id { get; set; }
    [Required] public virtual Guid ClientId { get; set; }
    [Required] public virtual Guid RefreshToken { get; set; }
    [Required] public virtual DateTime RefreshTokenExpires { get; set; }
    [Required] public virtual string Username { get; set; }
}