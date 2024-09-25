using Forum.Core.Repositories.Interfaces;
using Forum.EF;
using Forum.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Repositories;

public class CommentRepository : GenericRepository<Comment, int>, ICommentRepository
{
    public CommentRepository(ForumContext context) : base(context)
    {
    }

    public async Task<Comment> GetComment(int id)
    {
        return await _dbSet.Include(c => c.User).Include(c => c.Room).Where(c => c.IsPosted == true && c.IsDeleted == false && c.Room.IsOpened == true).FirstAsync();
    }

    public async Task<IEnumerable<Comment>> GetUnposted()
    {
        return await _dbSet.Include(c => c.User).Include(c => c.Room).Where(c => c.IsHandled == false && c.Room.IsOpened == true).ToListAsync();
    }
}
