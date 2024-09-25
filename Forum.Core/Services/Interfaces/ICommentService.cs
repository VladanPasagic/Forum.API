using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.EF.Entities;

namespace Forum.Core.Services.Interfaces;

public interface ICommentService : IGenericService<CommentResponse, CommentRequest, int>
{
    Task AddPost(string userId, CommentInRoomRequest postInChannel);

    Task<IEnumerable<CommentResponse>> GetUnposted();

    Task<CommentResponse> GetCommentAsync(int id);

    Task<Comment> GetComment(int id);

    Task ApproveComment(int id);

    Task DenyComment(int id);

    Task DeleteComment(int id);
}
