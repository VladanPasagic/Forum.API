using Forum.Core.Requests;
using Forum.Core.Responses;

namespace Forum.Core.Services.Interfaces;

public interface IUserService : IGenericService<UserResponse, SingleUserRequest, string>
{
    Task ApproveUser(string id);

    Task DenyUser(string id);

    Task<IEnumerable<UserResponse>> GetRequests();

    Task<IEnumerable<UserResponse>> GetUsers();

    Task<SingleUserResponse> GetSingle(string id);

    Task UpdateUserPermissions(string id, SingleUserRequest request);

}
