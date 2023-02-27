using Amazon.SecretsManager;
using Amazon.SimpleEmail.Model;
using Amazon.SQS.Model;
using comms_service.Application.DTO;
using comms_service.Application.helper;
using comms_service.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace comms_service.Application.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/sendtemplatedemail")]
    [ApiVersion("1.0")]
    public class SendTemplatedEmailController : ControllerBase
    {
        public SendTemplatedEmailController(IAmazonSecretsManager secretsManager)
        {
            var app_settings = new AppSettings(secretsManager);
        }
        [HttpPost]
        public async Task<ResponseDTO> SendTemplatedEmail(SendTemplatedEmailDTO req)
        {
            var request = new SendMessageRequest
            {
                QueueUrl = AppSettings.GetQueueUrl(),
                MessageBody = "templated email",
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "sendFrom", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = req.sendFrom
                        }
                    },
                    {
                        "sendTo", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = req.sendTo
                        }
                    },
                    {
                        "templateName", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = req.templateName
                        }
                    },
                    {
                        "templateData", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = req.templateData
                        }
                    }
                }
            };
            var response = await AmazonSQS.GetSQSClient().SendMessageAsync(request);
            var status = (int)response.HttpStatusCode;
            var res = new ResponseDTO
            {
                status_code = status,
                message = (status >= 200 && status < 300) ? "successfully sent the message" : (status >= 400 && status < 500 ? "User Error" : "Internal Server Error")
            };
            return res;
        }
    }
}