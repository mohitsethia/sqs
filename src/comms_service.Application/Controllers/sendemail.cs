using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http;
using System.Web;
using Amazon;
using comms_service.Application.DTO;
using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using comms_service.Application.Service;
using Amazon.SQS;
using Amazon.SQS.Model;
using comms_service.Application.Configs;
using comms_service.Application.helper;
using comms_service.Application.Infrastructure;

namespace comms_service.Application.Controllers{

    [ApiController]
    [Route("/api/v{version:apiVersion}/sendemail")]
    [ApiVersion("1.0")]

    public class SendEmailController : ControllerBase
    {
        private readonly ILogger<SendEmailController> _logger;
        private readonly AmazonSQSClient _sqsClient;
        public SendEmailController(ILogger<SendEmailController> logger, IAmazonSecretsManager secretsManager) {
            _logger = logger;
            var _appSettings = new AppSettings(secretsManager);
            _sqsClient = AmazonSQS.GetSQSClient();
        }

        [HttpPost]
        public async Task<ResponseDTO> SendEmail(SendEmailRequestDTO req)
        {
            var senderEmail = $"{req.SenderEmail}";
            var receiverEmail = $"{req.ReceiverEmail}";
            
            if(senderEmail == "" || receiverEmail == "")
            {
                var e = new ResponseDTO
                {
                    status_code = 400,
                    message = "please fill in all the details"
                };
                return e;
            }

            var request = new SendMessageRequest
            {
                QueueUrl = AppSettings.GetQueueUrl(),
                MessageBody = req.Body,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "sendFrom", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = senderEmail
                        }
                    },
                    {
                        "sendTo", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = receiverEmail
                        }
                    },
                    {
                        "emailSubject", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = req.Subject
                        }
                    },
                    {
                        "htmlBody", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = req.htmlBody
                        }
                    },
                    {
                        "textBody", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = req.textBody
                        }
                    }
                }
            };

            // Console.WriteLine($"{JsonConvert.SerializeObject(request)}");
            var response = await _sqsClient.SendMessageAsync(request);
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