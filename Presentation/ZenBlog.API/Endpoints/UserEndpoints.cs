using MediatR;
using ZenBlog.Application.Features.Users.Commands;

namespace ZenBlog.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void RegisterUserEndpoints(this IEndpointRouteBuilder app)
        {

            var users = app.MapGroup("/users").WithTags("Users");
            users.MapPost("/register",async (IMediator _mediator, CreateUserCommand command)=>
            {
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

        }
    }

}
