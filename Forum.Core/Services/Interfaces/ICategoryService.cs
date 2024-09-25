using Forum.Core.Requests;
using Forum.Core.Responses;

namespace Forum.Core.Services.Interfaces;

public interface ICategoryService : IGenericService<CategoryResponse, CategoryRequest, int>
{
}
