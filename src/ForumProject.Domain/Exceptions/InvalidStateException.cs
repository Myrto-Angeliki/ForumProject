namespace ForumProject.Domain.Exceptions;
public class InvalidStateException : DomainException
{
    public InvalidStateException(string message)
        : base(message)
    {
    }
}