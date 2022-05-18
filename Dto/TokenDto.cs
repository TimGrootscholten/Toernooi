using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class TokenDto : BaseDto
{
    public Guid Id { get; set; }
    [Required] public Guid ClientId { get; set; }
    [Required] public Guid RefreshToken { get; set; }
    [Required] public DateTime RefreshTokenExpires { get; set; }
    [Required] public string Username { get; set; }
}