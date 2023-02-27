using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Mvc;

namespace comms_service.Application.helper;

public static class SecretsManager
{
    
    public static async Task<string> GetSecret(IAmazonSecretsManager secretsManager, string secretId)
    {
        GetSecretValueRequest request = new GetSecretValueRequest();
        request.SecretId = secretId;
        request.VersionStage = "AWSCURRENT";
        GetSecretValueResponse response = await  secretsManager.GetSecretValueAsync(request);
        return response.SecretString;
    }
}