using FundooMicroservices.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Shared.Interfaces
{
    public interface IJWTService
    {
        string GenerateJWT(string email, string userID);

        JWTResponse ValidateToken(HttpRequest request);

    }
}
