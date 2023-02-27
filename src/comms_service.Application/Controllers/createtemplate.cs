using Amazon.SecretsManager;
using Amazon.SimpleEmail.Model;
using comms_service.Application.DTO;
using comms_service.Application.helper;
using comms_service.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace comms_service.Application.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/template")]
    [ApiVersion("1.0")]
    public class CreateTemplateController : ControllerBase
    {
        public CreateTemplateController(IAmazonSecretsManager secretsManager)
        {
            var app_settings = new AppSettings(secretsManager);
        }
        [HttpPost]
        public ActionResult<ResponseDTO> CreateTemplate(CreateTemplateDTO req)
        {
            //Create a TemplateRequest Object
            var request = new CreateTemplateRequest
            {
                Template = new Template
                {
                    TemplateName = req.templateName,
                    HtmlPart = $"<html><head></head><body>{req.htmlPart}</body></html>",
                    TextPart = req.textPart,
                    SubjectPart = req.subjectPart,
                },
            };

            // Send the request
            var response = AmazonSimpleEmailService.GetAmazonSESClient().CreateTemplateAsync(request);
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