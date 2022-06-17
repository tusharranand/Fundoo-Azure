using System.Collections.Generic;
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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Dapr.AzureFunctions.Extension;

namespace FundooMicroservices.FunctionApp.Notes
{
    public class NoteFunctions
    {
        private readonly ILogger<NoteFunctions> _logger;
        private readonly INoteServices _note;
        private readonly IJWTService _jwtService;

        public NoteFunctions(ILogger<NoteFunctions> log, INoteServices note, IJWTService jwtService)
        {
            _logger = log;
            _note = note;
            _jwtService = jwtService;
        }
        [FunctionName("AddNoteAsync")]
        [OpenApiOperation(operationId: "AddNoteAsync", tags: new[] { "Note" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(NoteModel), Required = true, Description = "New note details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NoteModel), Description = "The OK response")]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        public async Task<IActionResult> AddNoteAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notes")] HttpRequest req,
            [DaprState("%StateStoreName%", Key = "Notes")] IAsyncCollector<NoteModel> state)
        {
            var jwtResponse = _jwtService.ValidateToken(req);
            if (!jwtResponse.IsAuthorized)
                return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<NoteModel>(requestBody);

            var response = _note.AddNoteAsync(data, jwtResponse.Email);
            response = JsonConvert.DeserializeObject<NoteModel>(response);
            await state.AddAsync(response);

            return new OkObjectResult(response);
        }

        [FunctionName("GetAllNotes")]
        [OpenApiOperation(operationId: "GetAllNotes", tags: new[] { "Note" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NoteModel), Description = "The OK response")]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        public async Task<IActionResult> GetAllNotes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getNotes")] HttpRequest req,
            [DaprState("%StateStoreName%", Key = "Notes")] IAsyncCollector<NoteModel> state)
        {
            var jwtResponse = _jwtService.ValidateToken(req);
            if (!jwtResponse.IsAuthorized)
                return new UnauthorizedResult();

            var response = _note.GetNotes(jwtResponse.Email);

            return new OkObjectResult(state);
        }

        [FunctionName("UpdateNoteAsync")]
        [OpenApiOperation(operationId: "UpdateNoteAsync", tags: new[] { "Note" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(NoteModel), Required = true, Description = "Updated details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        public async Task<IActionResult> UpdateNoteAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "updateNotes")] HttpRequest req)
        {
            var jwtResponse = _jwtService.ValidateToken(req);
            if (!jwtResponse.IsAuthorized)
                return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<NoteModel>(requestBody);

            List<NoteModel> List = _note.UpdateNoteAsync(data, jwtResponse.Email);
            var response = $"Note updated from\n{List[0]}\nto\n{List[1]}";

            return new OkObjectResult(response);
        }

        [FunctionName("DeleteNoteAsync")]
        [OpenApiOperation(operationId: "DeleteNoteAsync", tags: new[] { "Note" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(string), Required = true, Description = "Note ID")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        public async Task<IActionResult> DeleteNoteAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "deleteNotes")] HttpRequest req)
        {
            var jwtResponse = _jwtService.ValidateToken(req);
            if (!jwtResponse.IsAuthorized)
                return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var response = _note.DeleteNoteAsync(requestBody, jwtResponse.Email);

            return new OkObjectResult(response);
        }
    }
}

