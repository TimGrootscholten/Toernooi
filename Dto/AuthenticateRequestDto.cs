namespace Dtos
{
    public class AuthenticateRequestDto
    {
        public Guid ClientId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
