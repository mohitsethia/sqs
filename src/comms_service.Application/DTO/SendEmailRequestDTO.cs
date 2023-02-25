namespace comms_service.Application.DTO;

public class SendEmailRequestDTO
{
    public string SenderEmail { get; init; }
    public string SenderName { get; init; }
    public string ReceiverEmail { get; init; }
    public string ReceiverName { get; init; }
}