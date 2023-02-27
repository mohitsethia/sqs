using Amazon;
using Amazon.Internal;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using comms_service.Application.helper;

namespace comms_service.Application.Infrastructure;

public static class AmazonSimpleEmailService
{
    private static readonly AmazonSimpleEmailServiceClient _amazonSimpleEmailServiceClient;

    static AmazonSimpleEmailService()
    {
        var accessKeyID = AppSettings.GetAccessKeyId();
        var secretKey = AppSettings.GetSecretAcessKey();
        var credentials = new BasicAWSCredentials(accessKeyID, secretKey);
        var client = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.APSoutheast1);
        _amazonSimpleEmailServiceClient = client;
    }

    public static AmazonSimpleEmailServiceClient GetAmazonSESClient()
    {
        return _amazonSimpleEmailServiceClient;
    }
}