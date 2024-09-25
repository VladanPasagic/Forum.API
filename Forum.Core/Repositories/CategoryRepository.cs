using Forum.Core.Repositories.Interfaces;
using Forum.EF;
using Forum.EF.Entities;

namespace Forum.Core.Repositories;

public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
{
    public CategoryRepository(ForumContext context) : base(context)
    {
    }

}
