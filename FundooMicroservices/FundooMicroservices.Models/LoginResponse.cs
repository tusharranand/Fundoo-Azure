using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Models
{
    public class LoginResponse
    {
        public UserModel UserDetails { get; set; }

        public string token { get; set; }

    }
}
