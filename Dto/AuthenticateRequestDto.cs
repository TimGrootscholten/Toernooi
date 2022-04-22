namespace Dtos
{
    public class AuthenticateRequestDto
    {
        public Guid ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
