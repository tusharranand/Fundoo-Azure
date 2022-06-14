using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Models
{
    public class JWTResponse
    {
        public bool IsAuthorized { get; set; }
        public string Email { get; set; }
        public string userID { get; set; }
    }
}
