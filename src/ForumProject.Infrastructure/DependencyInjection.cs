using ForumProject.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ForumProject.Infrastructure.Persistence;
using ForumProject.Infrastructure.Repositories;
using ForumProject.Application.Features.Comments.Interfaces;
using ForumProject.Application.Features.Posts.Interfaces;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Application.Features.Auths.Interfaces;
using ForumProject.Application.Features.Friendships.Interfaces;

namespace ForumProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

        services.AddSingleton(new DataContextDapper(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        return services;
    }
}
