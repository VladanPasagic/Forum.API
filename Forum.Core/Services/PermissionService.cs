using AutoMapper;
using Forum.Core.Repositories.Interfaces;
using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.Extensions.Logging;

namespace Forum.Core.Services;

public class PermissionService : GenericService<IPermissionRepository, PermissionService, PermissionRequest, PermissionResponse, Permission, int>, IPermissionService
{
    public PermissionService(IPermissionRepository repository, ILogger<PermissionService> logger, IMapper mapper) : base(repository, logger, mapper)
    {
    }

    public async Task<IEnumerable<PermissionResponse>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<PermissionResponse>>(await _repository.GetAll());
    }

    public override Task Update(int id, PermissionRequest entity)
    {
        throw new NotImplementedException();
    }
}
