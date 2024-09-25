using Forum.EF.Entities;

namespace Forum.Core.Repositories.Interfaces;

public interface IRoomRepository : IGenericRepository<Room, int>
{
    Task<IEnumerable<Room>> GetAllByCategoryId(int categoryId);

    Task<IEnumerable<Comment>> GetAllComments(int id);

    Task<Room> GetAsync(int id);

    Task<IEnumerable<Room>> GetAllOpenedRooms();

    Task<IEnumerable<Room>> GetUnopened();

    Task<Room> GetAsyncUnapproved(int id);
}
