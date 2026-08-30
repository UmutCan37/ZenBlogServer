using MediatR;
using ZenBlog.Application.Features.Categories.Queries;

namespace ZenBlog.API.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void RegisterCategoryRootes(this IEndpointRouteBuilder app)
        {
            var categories = app.MapGroup("/categories").WithTags("Categories");

            categories.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryQuery());
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}
