namespace WorkQueue.Core.Messaging.Settings;

public sealed record RabbitMqSettings
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string VirtualHost { get; set; } = string.Empty;
}