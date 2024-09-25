using Forum.EF.Entities;

namespace Forum.Core.Repositories.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment, int>
{

    Task<IEnumerable<Comment>> GetUnposted();

    Task<Comment> GetComment(int id);
}
