using AutoMapper;
using Forum.Core.Repositories.Interfaces;
using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.Extensions.Logging;

namespace Forum.Core.Services;

public class CommentService : GenericService<ICommentRepository, CommentService, CommentRequest, CommentResponse, Comment, int>, ICommentService
{
    public CommentService(ICommentRepository repository, ILogger<CommentService> logger, IMapper mapper) : base(repository, logger, mapper)
    {
    }

    public async Task AddPost(string userId, CommentInRoomRequest postInChannel)
    {
        var post = _mapper.Map<Comment>(postInChannel);
        post.PublishingDateTime = DateTime.UtcNow;
        post.UserId = userId;
        await _repository.AddAsync(post);
    }

    public async Task ApproveComment(int id)
    {
        var comment = await _repository.GetByIdAsync(id);
        comment.IsHandled = true;
        comment.IsPosted = true;
        await _repository.UpdateAsync(comment);
    }

    public async Task DeleteComment(int id)
    {
        var comment = await _repository.GetComment(id);
        comment.IsDeleted = true;
        await _repository.UpdateAsync(comment);
    }

    public async Task DenyComment(int id)
    {
        var comment = await _repository.GetByIdAsync(id);
        comment.IsHandled = true;
        comment.IsPosted = false;
        await _repository.UpdateAsync(comment);
    }

    public Task<Comment> GetComment(int id)
    {
        return _repository.GetComment(id);
    }

    public async Task<CommentResponse> GetCommentAsync(int id)
    {
        return _mapper.Map<CommentResponse>(await _repository.GetComment(id));
    }

    public async Task<IEnumerable<CommentResponse>> GetUnposted()
    {
        return _mapper.Map<IEnumerable<CommentResponse>>(await _repository.GetUnposted());
    }

    public async override Task Update(int id, CommentRequest entity)
    {
        var comment = await _repository.GetComment(id);
        comment.Content = entity.Content;
        await _repository.UpdateAsync(comment);
    }
}
