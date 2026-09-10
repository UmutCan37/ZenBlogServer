using MediatR;
using ZenBlog.Application.Features.Blogs.Commands;
using ZenBlog.Application.Features.Blogs.Queries;

namespace ZenBlog.API.Endpoints
{
    public static class BlogEndpoints
    {
        public static void RegisterBlogEndpoints(this IEndpointRouteBuilder app)
        {
            var blogs = app.MapGroup("/blogs").WithTags("Blogs");

            blogs.MapGet(string.Empty, async (IMediator _mediator) =>
            {
                var response = await _mediator.Send(new GetBlogsQuery());
                return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
            });

            blogs.MapPost(string.Empty,async(CreateBlogCommand command, IMediator _mediator) =>
            {
                var response = await _mediator.Send(command);
                return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
            });

            blogs.MapGet("/{id}", async (Guid id, IMediator _mediator) =>
            {
                var response = await _mediator.Send(new GetBlogByIdQuery(id));
                return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
            });
        }
    }
}
