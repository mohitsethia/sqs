namespace comms_service.Application.helper;

public class AppSettings
{
    public string SecretAcessKey { get; set; }
    public string AccessKeyID { get; set; }
    public string EmailFrom { get; set; }
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPass { get; set; }
}