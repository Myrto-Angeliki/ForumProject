namespace ForumProject.Api.Exceptions;

public static class ExceptionHandlerExtensions
{
    public static void AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<DomainExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

    }
}