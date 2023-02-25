using SendGrid;
using SendGrid.Helpers.Mail;

namespace comms_service.Application.Service;

public class SendEmailService
{
    public Task<Response> SendEmail(string name, string email)
    {
        // _logger.LogInformation($"(Element creation) Element creation requested => '{JsonConvert.SerializeObject(req)}'");
        // var request = JsonConvert.SerializeObject(req);
        // Console.WriteLine("printing request body: {0}", request);
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("mohitjain1998@gmail.com", "Example User");
        var subject = "Sending with SendGrid is Fun";
        var to = new EmailAddress(email, name);
        var plainTextContent = "and easy to do anywhere, even with C#";
        var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = client.SendEmailAsync(msg);
        return response;
    }
}