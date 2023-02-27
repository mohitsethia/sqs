using Amazon.SecretsManager;

namespace comms_service.Application.helper;

public class AppSettings
{
    private static string _secretAcessKey;
    private static string _accessKeyID;
    private static string _queueUrl;

    private readonly IAmazonSecretsManager _secretsManager;
    public AppSettings(IAmazonSecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
        _secretAcessKey = SecretsManager.GetSecret(_secretsManager, "secret-access-key").Result;
        _accessKeyID = SecretsManager.GetSecret(_secretsManager,"access-key-id").Result;
        _queueUrl = SecretsManager.GetSecret(_secretsManager, "test-queue-url").Result;
    }

    public static string GetAccessKeyId()
    {
        return _accessKeyID;
    }

    public static string GetSecretAcessKey()
    {
        return _secretAcessKey;
    }

    public static string GetQueueUrl()
    {
        return _queueUrl;
    }
}