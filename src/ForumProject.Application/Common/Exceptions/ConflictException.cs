namespace ForumProject.Application.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) 
            : base(message) { }

        public ConflictException(string entityName, string propertyName, object value) 
            : base($"{entityName} with {propertyName} '{value}' already exists.") { }
    }
}