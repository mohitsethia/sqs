namespace comms_service.Application.Configs;

public static class AppConfig
{
    public static readonly IConfigurationSection app_settings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings");
}