using Forum.EF.Entities;

namespace Forum.Core.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User, string>
{
    Task<IEnumerable<User>> GetRequests();

    Task<IEnumerable<User>> GetUsers();

    Task<User> GetById(string id);

}
