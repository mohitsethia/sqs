namespace comms_service.Application.DTO;

public class SendEmailRequestDTO
{
    public string SenderEmail { get; set; }
    // public string SenderName { get; set; }
    public string ReceiverEmail { get; set; }
    // public string ReceiverName { get; set; }
    public string Body { get; init; }
    public string Subject { get; set; }
    public string htmlBody { get; set; }
    public string textBody { get; set; }
}