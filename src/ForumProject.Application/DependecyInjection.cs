using ForumProject.Application.Features.Auths.Interfaces;
using ForumProject.Application.Features.Auths.Services;
using ForumProject.Application.Features.Comments.Interfaces;
using ForumProject.Application.Features.Comments.Services;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.FriendRequests.Services;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Friendships.Services;
using ForumProject.Application.Features.Posts.Interfaces;
using ForumProject.Application.Features.Posts.Services;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Application.Features.Users.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForumProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(new AuthHelperService(configuration));
            
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFriendRequestService, FriendRequestService>();
            services.AddScoped<IFriendshipService, FriendshipService>();

            return services;
        }
    }
}