using Forum.EF.Entities;

namespace Forum.Core.Repositories.Interfaces;

public interface IPermissionRepository : IGenericRepository<Permission, int>
{
    Task<IEnumerable<Permission>> GetAll();
}
