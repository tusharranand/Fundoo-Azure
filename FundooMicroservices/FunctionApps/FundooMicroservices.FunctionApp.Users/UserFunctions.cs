using System.IO;
using System.Net;
using System.Threading.Tasks;
using FundooMicroservices.Models;
using FundooMicroservices.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FundooMicroservices.FunctionApp.Users
{
    public class UserFunctions
    {
        private readonly ILogger<UserFunctions> _logger;
        private readonly IUserServices _user;

        public UserFunctions(ILogger<UserFunctions> log, IUserServices user)
        {
            _logger = log;
            _user = user;
        }

        [FunctionName("UserRegistration")]
        [OpenApiOperation(operationId: "UserRegistration", tags: new[] { "Users" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserModel), Required = true, Description = "New user details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserModel), Description = "The OK response")]
        public async Task<IActionResult> UserRegistration(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<UserModel>(requestBody);

            var response = this._user.UserRegistration(data);

            return new OkObjectResult(response);
        }

        [FunctionName("GetUser")]
        [OpenApiOperation(operationId: "GetUser", tags: new[] { "Users" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserModel), Description = "The OK response")]
        public async Task<IActionResult> GetUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getUsers")] HttpRequest req)
        {
            var response = this._user.GetUsers();

            return new OkObjectResult(response);
        }

        [FunctionName("UserLogin")]
        [OpenApiOperation(operationId: "UserLogin", tags: new[] { "Users" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginCredentials), Required = true, Description = "Login Credentials")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResponse), Description = "The OK response")]
        public async Task<IActionResult> UserLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "loginUsers")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<LoginCredentials>(requestBody);

            var response = this._user.UserLogin(data);

            return new OkObjectResult(response);
        }
    }
}

