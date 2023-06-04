namespace Fanout.Core.Exceptions;

public sealed class DevelopmentException : Exception
{
    public DevelopmentException(string message) : base(message)
    {
    }
}
