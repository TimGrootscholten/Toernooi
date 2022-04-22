using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApiException : Exception
    {
        public virtual HttpStatusCode Code { get; set; }
        public virtual string Description { get; set; }

        public ApiException(HttpStatusCode code, string description = "")
        {
            Code = code;
            Description = description;
        }
    }
}
