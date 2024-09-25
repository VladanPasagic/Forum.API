using Forum.Core.Requests;
using Forum.Core.Responses;

namespace Forum.Core.Services.Interfaces;

public interface IPermissionService : IGenericService<PermissionResponse, PermissionRequest, int>
{
    Task<IEnumerable<PermissionResponse>> GetAllAsync();
}
