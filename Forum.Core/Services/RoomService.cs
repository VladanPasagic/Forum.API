using AutoMapper;
using Forum.Core.Repositories.Interfaces;
using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.Extensions.Logging;

namespace Forum.Core.Services;

public class RoomService : GenericService<IRoomRepository, RoomService, RoomRequest, RoomResponse, Room, int>, IRoomService
{
    private readonly ICommentRepository _commentRepository;

    public RoomService(IRoomRepository repository, ILogger<RoomService> logger, IMapper mapper, ICommentRepository commentRepository) : base(repository, logger, mapper)
    {
        _commentRepository = commentRepository;
    }

    public async Task CreateNew(string userId, RoomRequest request)
    {
        Room room = _mapper.Map<Room>(request);
        room.CreatedDate = DateTime.UtcNow;
        room.UserId = userId;
        await _repository.AddAsync(room);

        Comment comment = new()
        {
            RoomId = room.Id,
            Content = request.Content,
            PublishingDateTime = DateTime.UtcNow,
            UserId = userId
        };
        await _commentRepository.AddAsync(comment);
    }

    public async Task<IEnumerable<RoomResponse>> GetAllByCategoryId(int categoryId)
    {
        return _mapper.Map<IEnumerable<RoomResponse>>(await _repository.GetAllByCategoryId(categoryId));
    }

    public async Task<IEnumerable<CommentResponse>> GetAllComments(int id)
    {
        return _mapper.Map<IEnumerable<CommentResponse>>(await _repository.GetAllComments(id));
    }

    public async Task<IEnumerable<RoomResponse>> GetAllOpenedRooms()
    {
        return _mapper.Map<IEnumerable<RoomResponse>>(await _repository.GetAllOpenedRooms());
    }

    public async Task<RoomResponse> GetSingleRoom(int id)
    {
        return _mapper.Map<RoomResponse>(await _repository.GetAsync(id));
    }

    public async Task<IEnumerable<RoomResponse>> GetUnopened()
    {
        return _mapper.Map<IEnumerable<RoomResponse>>(await _repository.GetUnopened());
    }

    public async Task ApproveRoom(int id)
    {
        var room = await _repository.GetAsyncUnapproved(id);
        room.IsHandled = true;
        room.IsOpened = true;
        await _repository.UpdateAsync(room);
    }

    public async Task DenyRoom(int id)
    {
        var room = await _repository.GetAsyncUnapproved(id);
        room.IsHandled = true;
        room.IsOpened = false;
        await _repository.UpdateAsync(room);
    }

    public async Task UpdateRoom(int id, RoomUpdateRequest request)
    {
        await Update(id, _mapper.Map<RoomRequest>(request));
    }

    public async Task DeleteRoom(int id)
    {
        var room = await _repository.GetAsync(id);
        room.IsDeleted = true;
        await _repository.UpdateAsync(room);
    }

    public override async Task Update(int id, RoomRequest entity)
    {
        var room = await _repository.GetAsync(id);
        room.Description = entity.Description;
        room.CategoryId = entity.CategoryId;
        room.Title = entity.Title;
        await _repository.UpdateAsync(room);
    }

    public async Task<RoomResponse> GetSingleRoomUnopened(int id)
    {
        return _mapper.Map<RoomResponse>(await _repository.GetAsyncUnapproved(id));
    }

    public async Task UpdateModerator(int id, RoomUpdateRequest request)
    {
        var room = await _repository.GetAsyncUnapproved(id);
        room.Description = request.Description;
        room.CategoryId = request.CategoryId;
        room.Title = request.Title;
        await _repository.UpdateAsync(room);
    }
}
