namespace Headers.Core.Messaging.Settings;

public sealed record RabbitMqSettings
{
    public string Username { get; init; }  = string.Empty;
    public string Password { get; init; }  = string.Empty;
    public string Host { get; init; }  = string.Empty;
    public string VirtualHost { get; init; } = string.Empty;
    public int Port { get; init; }
}