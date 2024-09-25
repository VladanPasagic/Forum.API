using Forum.Core.Repositories.Interfaces;
using Forum.EF;
using Forum.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Repositories;

public class UserRepository : GenericRepository<User, string>, IUserRepository
{

    public UserRepository(ForumContext context) : base(context)
    {
    }

    public async Task<User> GetById(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetRequests()
    {
        return await _context.Users.Where(u => u.IsHandled == false).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users.Where(u => u.IsHandled == true && u.EmailConfirmed==true).ToListAsync();
    }
}
