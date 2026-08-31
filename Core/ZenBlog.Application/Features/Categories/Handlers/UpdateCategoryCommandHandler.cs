using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZenBlog.Application.Base;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Application.Features.Categories.Commands;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Categories.Handlers
{
    public class UpdateCategoryCommandHandler(IRepository<Category> repository,IMapper mapper,IUnitOfWork unitOfWork ) : IRequestHandler<UpdateCategoryCommand, BaseResult<bool>>
    {
        public async Task<BaseResult<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category=mapper.Map<Category>(request);
            repository.Update(category);
            var result = await unitOfWork.SaveChangesAsync();

            return result  ? BaseResult<bool>.Success(true) : BaseResult<bool>.Fail("Failed to update category");
        }

    }
}
