using AutoMapper;
using Forum.Core.Repositories.Interfaces;
using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.Extensions.Logging;

namespace Forum.Core.Services;

public class CategoryService :
    GenericService<ICategoryRepository, CategoryService, CategoryRequest, CategoryResponse, Category, int>, ICategoryService
{
    public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger, IMapper mapper) : base(repository, logger, mapper)
    {
    }

    public override Task Update(int id, CategoryRequest entity)
    {
        throw new NotImplementedException();
    }
}
