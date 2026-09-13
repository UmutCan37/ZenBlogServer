using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZenBlog.Application.Base;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Application.Features.Users.Queries;
using ZenBlog.Application.Features.Users.Result;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Users.Handlers
{
    public class GetLoginQueryHandler : IRequestHandler<GetLoginQuery, BaseResult<GetLoginQueryResult>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public GetLoginQueryHandler(UserManager<AppUser> userManager, IJwtService jwtService, IMapper mapper)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<BaseResult<GetLoginQueryResult>> Handle(GetLoginQuery request, CancellationToken cancellationToken)
        {
            var user =await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BaseResult<GetLoginQueryResult>.NotFound("User not found");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return BaseResult<GetLoginQueryResult>.Fail("Invalid password");
            }
            var userResult = _mapper.Map<GetUsersQueryResult>(user);
            var response = await _jwtService.GenerateTokenAsync(userResult);

            if (response == null)
            {
                return BaseResult<GetLoginQueryResult>.Fail("Token generation failed");
            }

            return BaseResult<GetLoginQueryResult>.Success(response);

        }
    }
}
