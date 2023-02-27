using Amazon.SecretsManager;
using Amazon.SimpleEmail.Model;
using comms_service.Application.DTO;
using comms_service.Application.helper;
using comms_service.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SendGrid;

namespace comms_service.Application.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/template")]
    [ApiVersion("1.0")]
    public class DeleteTemplateController : ControllerBase
    {
        public DeleteTemplateController(IAmazonSecretsManager secretsManager)
        {
            var app_settings = new AppSettings(secretsManager);
        }
        [HttpDelete]
        public ActionResult<ResponseDTO> DeleteTemplate(DeleteTemplateDTO req)
        {
            var request = new DeleteTemplateRequest
            {
                TemplateName = req.templateName
            };
            var response = AmazonSimpleEmailService.GetAmazonSESClient().DeleteTemplateAsync(request);
            var status = (int)response.Result.HttpStatusCode;
            var res = new ResponseDTO
            {
                status_code = status,
                message = (status >= 200 && status < 300) ? "successfully sent the message" : (status >= 400 && status < 500 ? "User Error" : "Internal Server Error")
            };
            return res;
        }
    }
}