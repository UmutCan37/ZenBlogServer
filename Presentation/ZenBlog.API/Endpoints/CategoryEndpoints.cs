using MediatR;
using ZenBlog.Application.Features.Categories.Commands;
using ZenBlog.Application.Features.Categories.Queries;

namespace ZenBlog.API.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void RegisterCategoryRootes(this IEndpointRouteBuilder app)
        {
            var categories = app.MapGroup("/categories").WithTags("Categories");

            categories.MapGet(string.Empty, async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryQuery());
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            categories.MapPost("/", async (IMediator mediator, CreateCategoryCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            categories.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id));
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            categories.MapPut(string.Empty, async (IMediator mediator, UpdateCategoryCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
            categories.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
            {
                var result = await mediator.Send(new RemoveCategoryCommand(id));
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }
}
