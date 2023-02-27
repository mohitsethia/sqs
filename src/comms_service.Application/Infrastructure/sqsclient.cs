using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using comms_service.Application.helper;

namespace comms_service.Application.Infrastructure;

public static class AmazonSQS
{
    private static readonly AmazonSQSClient _sqsClient;

    static AmazonSQS()
    {
        var accessKeyID = AppSettings.GetAccessKeyId();
        var secretKey = AppSettings.GetSecretAcessKey();
        var credentials = new BasicAWSCredentials(accessKeyID, secretKey);
        var sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.APSoutheast1);
        _sqsClient = sqsClient;
    }

    public static AmazonSQSClient GetSQSClient()
    {
        return _sqsClient;
    }
}