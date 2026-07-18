using ForumProject.Application.Features.Comments.Interfaces;
using ForumProject.Application.Features.Comments.Services;
using ForumProject.Application.Features.Posts.Interfaces;
using ForumProject.Application.Features.Posts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ForumProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPostService, PostService>();

            return services;
        }
    }
}