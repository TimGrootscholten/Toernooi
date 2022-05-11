using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class TeamDto : BaseDto
    {
        public virtual Guid Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
    }
}
