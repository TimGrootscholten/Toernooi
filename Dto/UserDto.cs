using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class UserDto : BaseDto
    {
        public virtual Guid Id { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? Password { get; set; }
        public virtual string? FirstName { get; set; }
        public virtual string? LastName { get; set; }
    }
}
