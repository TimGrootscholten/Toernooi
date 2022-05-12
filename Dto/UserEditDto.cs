using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class UserEditDto : BaseDto
{
    public Guid Id { get; set; }
    [Required] public string Username { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
}