using FundooMicroservices.Models;
using FundooMicroservices.Shared.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Shared.Services
{
    public class UserServices : IUserServices
    {
        private readonly IJWTService _jWTService;
        private readonly ISettingServices _settingServices;

        // Cosmos DocDB API database
        private string _docDbEndpointUri;
        private string _docDbPrimaryKey;
        private string _docDbDatabaseName;

        // Doc DB Collections
        private string _docDbDigitalMainCollectionName;

        private static CosmosClient _docDbSingletonClient;
        private readonly Container _cosmosContainer;

        public UserServices(ISettingServices settingServices, IJWTService jWTService)
        {
            _settingServices = settingServices;
            _jWTService = jWTService;

            _docDbEndpointUri = _settingServices.GetDocDbEndpointUri();
            _docDbPrimaryKey = _settingServices.GetDocDbApiKey();
            _docDbDatabaseName = _settingServices.GetDocDbDatabaseName();
            _docDbDigitalMainCollectionName = _settingServices.GetDocDbMainCollectionName();
            _docDbSingletonClient = new CosmosClient(_settingServices.GetDocDbEndpointUri(), settingServices.GetDocDbApiKey());
            _cosmosContainer = _docDbSingletonClient.GetContainer(_docDbDatabaseName, _docDbDigitalMainCollectionName);

        }

        public async Task<UserModel> UserRegistration(UserModel newUserDetails)
        {
            try
            {
                if (newUserDetails == null)
                    throw new ArgumentNullException(nameof(newUserDetails));

                newUserDetails.Id = Guid.NewGuid().ToString();
                newUserDetails.RegisterDate = DateTime.Now;
                using (var response = _cosmosContainer
                    .CreateItemAsync<UserModel>(newUserDetails, 
                    new PartitionKey(newUserDetails.Email)))
                {
                    return response.Result.Resource;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserModel>> GetUsers()
        {
            try
            {
                var response = _cosmosContainer.GetItemLinqQueryable<UserModel>(true);
                return response.ToList<UserModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async  Task<LoginResponse> UserLogin(LoginCredentials userLoginDetails)
        {
            try
            {
                var user = _cosmosContainer.GetItemLinqQueryable<UserModel>(true)
                    .Where(x => x.Email == userLoginDetails.Email && 
                    x.Password == userLoginDetails.Password).AsEnumerable().FirstOrDefault();

                if (user == null)
                    throw new ArgumentNullException("Invalid Credentials");

                LoginResponse response = new LoginResponse();
                response.UserDetails = user;
                response.token = _jWTService.GenerateJWT(user.Email, user.Id);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
