using FundooMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Shared.Interfaces
{
    public interface IUserServices
    {
        Task<UserModel> UserRegistration(UserModel newUserDetails);

        Task<List<UserModel>> GetUsers();

        Task<LoginResponse> UserLogin(LoginCredentials userLoginDetails);
    }
}
