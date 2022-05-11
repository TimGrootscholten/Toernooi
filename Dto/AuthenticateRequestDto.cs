using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class AuthenticateRequestDto
    {
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
