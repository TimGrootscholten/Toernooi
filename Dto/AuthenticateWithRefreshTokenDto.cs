using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class AuthenticateWithRefreshTokenDto
{
    [Required] public Guid ClientId { get; set; }
    [Required] public Guid RefreshToken { get; set; }
}