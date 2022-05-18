using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class TeamDto : BaseDto
{
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
}