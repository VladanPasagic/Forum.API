using Forum.Core.Repositories.Interfaces;
using Forum.EF;
using Forum.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Repositories;

public class RoomRepository : GenericRepository<Room, int>, IRoomRepository
{
    public RoomRepository(ForumContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Room>> GetAllByCategoryId(int categoryId)
    {
        return await _context.Rooms.Include(r => r.Category).Where(r => r.CategoryId == categoryId && r.IsOpened == true && r.IsDeleted == false).ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetAllComments(int id)
    {
        return await _context.Rooms.Include(r => r.Comments).ThenInclude(c => c.Room).Include(r => r.Comments).ThenInclude(c => c.User).Where(r => r.Id == id && r.IsOpened == true).SelectMany(r => r.Comments).Where(c => c.IsPosted == true && c.IsDeleted == false).OrderBy(c => c.PublishingDateTime).Take(20).ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetAllOpenedRooms()
    {
        return await _context.Rooms.Include(r => r.Category).Include(r => r.User).Where(r => r.IsOpened == true && r.IsDeleted == false).ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetUnopened()
    {
        return await _context.Rooms.Include(r => r.Category).Include(r => r.User).Where(r => r.IsHandled == false).ToListAsync();
    }

    public async Task<Room> GetAsync(int id)
    {
        return await _context.Rooms.Include(r => r.Category).Include(r => r.User).Where(r => r.Id == id && r.IsOpened == true && r.IsDeleted == false).FirstAsync();
    }

    public async Task<Room> GetAsyncUnapproved(int id)
    {
        return await _context.Rooms.Include(r => r.Category).Include(r => r.User).Where(r => r.Id == id && r.IsHandled == false).FirstAsync();
    }
}
