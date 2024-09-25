using Forum.Core.Requests;
using Forum.Core.Responses;

namespace Forum.Core.Services.Interfaces;

public interface IRoomService : IGenericService<RoomResponse, RoomRequest, int>
{
    Task CreateNew(string userId, RoomRequest request);

    Task<IEnumerable<RoomResponse>> GetAllByCategoryId(int categoryId);

    Task<IEnumerable<CommentResponse>> GetAllComments(int id);

    Task<IEnumerable<RoomResponse>> GetAllOpenedRooms();

    Task<RoomResponse> GetSingleRoom(int id);

    Task<IEnumerable<RoomResponse>> GetUnopened();

    Task ApproveRoom(int id);

    Task DenyRoom(int id);

    Task UpdateRoom(int id, RoomUpdateRequest request);

    Task DeleteRoom(int id);

    Task<RoomResponse> GetSingleRoomUnopened(int id);

    Task UpdateModerator(int id, RoomUpdateRequest request);
}
