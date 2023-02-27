using Newtonsoft.Json.Linq;
using ThirdParty.Json.LitJson;

namespace comms_service.Application.DTO;

public class SendTemplatedEmailDTO
{
    public string sendFrom { get; set; }
    public string sendTo { get; set; }
    public string templateName { get; set; }
    public string templateData { get; set; }
}