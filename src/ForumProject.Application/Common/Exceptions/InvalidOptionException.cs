namespace ForumProject.Application.Common.Exceptions;

public class InvalidOptionException : Exception
{
    public InvalidOptionException(string message) 
        : base(message) { }
}