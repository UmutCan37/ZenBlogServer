using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ZenBlog.Application.Features.Users.Commands;
using ZenBlog.Application.Features.Users.Queries;

namespace ZenBlog.Application.Features.Users.Endpoints
{
    public static class UserEndpoints
    {
        public static void RegisterUserEndpoints(this IEndpointRouteBuilder app)
        {

            var users = app.MapGroup("/users").WithTags("Users").AllowAnonymous();
            users.MapPost("/register",async (IMediator _mediator, CreateUserCommand command)=>
            {
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

            users.MapPost("login", async (IMediator _mediator, GetLoginQuery query) =>
            {
                var result = await _mediator.Send(query);
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });

        }
    }

}
