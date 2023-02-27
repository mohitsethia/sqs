using Amazon.SecretsManager;
using Amazon.SimpleEmail.Model;
using comms_service.Application.DTO;
using comms_service.Application.helper;
using comms_service.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace comms_service.Application.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/template")]
    [ApiVersion("1.0")]
    public class ListTemplateController : ControllerBase
    {
        public ListTemplateController(IAmazonSecretsManager secretsManager)
        {
            var app_settings = new AppSettings(secretsManager);
        }
        [HttpGet]
        public async Task<ResponseDTO> ListEmailTemplates()
        {
            var response = await AmazonSimpleEmailService.GetAmazonSESClient().ListTemplatesAsync(new ListTemplatesRequest());
            var status = (int)response.HttpStatusCode;
            var res = new ResponseDTO
            {
                status_code = status,
                message = JsonConvert.SerializeObject(response.TemplatesMetadata)
            };
            return res;
        } 
    }
}