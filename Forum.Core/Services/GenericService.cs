using AutoMapper;
using Forum.Core.Repositories.Interfaces;
using Forum.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Forum.Core.Services;

public abstract class GenericService<Repository, LoggerWrapper, Request, Response, Entity, PrimaryKeyType>
    : IGenericService<Response, Request, PrimaryKeyType> where Repository : IGenericRepository<Entity, PrimaryKeyType> where Entity : class
{
    protected Repository _repository;
    protected ILogger<LoggerWrapper> _logger;
    protected IMapper _mapper;

    protected GenericService(Repository repository, ILogger<LoggerWrapper> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Add(Request entity)
    {
        await _repository.AddAsync(_mapper.Map<Entity>(entity));
    }

    public async Task Delete(PrimaryKeyType id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Response?> Get(PrimaryKeyType id)
    {
        Entity? entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return default;
        }
        return _mapper.Map<Response>(entity);
    }

    public async Task<IEnumerable<Response>> GetAll()
    {
        return _mapper.Map<IEnumerable<Response>>(await _repository.GetAllAsync());
    }

    public abstract Task Update(PrimaryKeyType id, Request entity);
}
