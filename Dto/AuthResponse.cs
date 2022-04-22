using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class AuthResponse
    {
        public string AccesToken { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
