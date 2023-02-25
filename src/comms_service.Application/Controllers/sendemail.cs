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
using comms_service.Application.Service;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace comms_service.Application.Controllers{

    [ApiController]
    [Route("/api/v{version:apiVersion}/sendemail")]
    [ApiVersion("1.0")]

    public class SendEmailController : ControllerBase
    {
        private readonly ILogger<SendEmailController> _logger;
        private readonly IConfigurationSection app_settings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings");

        public SendEmailController(ILogger<SendEmailController> logger) {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<ResponseDTO> SendEmail(SendEmailRequestDTO req)
        {
            var senderEmail = $"{req.SenderEmail}";
            var senderName = $"{req.SenderName}";
            var receiverEmail = $"{req.ReceiverEmail}";
            var receiverName = $"{req.ReceiverName}";
            
            if(senderEmail == "" || senderName == "" || receiverEmail == "" || receiverName == "")
            {
                var e = new ResponseDTO
                {
                    status_code = 400,
                    message = "please fill in all the details"
                };
                return BadRequest(e);
            }

            var accessKeyID = app_settings["AccessKeyID"];
            var secretKey = app_settings["SecretAccessKey"];
            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);
            var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast1);
            var queueUrl = "https://sqs.us-east-1.amazonaws.com/363402790710/testqueue";

            var body = "Hey there, how are you doing?";
            var request = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = body,
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
                        "sendFromName", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = senderName
                        }
                    },
                    {
                        "sendToName", new MessageAttributeValue
                        {
                            DataType = "String", StringValue = receiverName
                        }
                    }
                }
            };

            var response = sqsClient.SendMessageAsync(request);
            var status = (int)response.Result.HttpStatusCode;
            var res = new ResponseDTO
            {
                status_code = status,
                message = (status >= 200 && status < 300) ? "successfully sent the message" : (status >= 400 && status < 500 ? "User Error" : "Internal Server Error")
            };
            return Ok(res);

            // var r = new SendEmailService();
            // var response = r.SendEmail(name, email);
            // Console.WriteLine($"printing the response result: '{JsonConvert.SerializeObject(response.Result)}'");
            // var result = new SendEmailDTO{status_code=(int)response.Result.StatusCode,
            // message=$"email successfully sent to {name} in the email address {email}"};
            // return Ok(result);
        }
    }
}