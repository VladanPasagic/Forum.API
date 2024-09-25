using Forum.Core.Repositories.Interfaces;
using Forum.EF;
using Forum.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Repositories;

public class PermissionRepository : GenericRepository<Permission, int>, IPermissionRepository
{
    public PermissionRepository(ForumContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Permission>> GetAll()
    {
        return await _context.Permissions.Include(c => c.Category).ToListAsync();
    }
}
