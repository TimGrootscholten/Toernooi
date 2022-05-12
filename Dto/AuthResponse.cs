using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class AuthResponse
{
    [Required] public string AccesToken { get; set; }
    [Required] public Guid RefreshToken { get; set; }
}