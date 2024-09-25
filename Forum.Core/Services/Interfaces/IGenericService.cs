namespace Forum.Core.Services.Interfaces;

public interface IGenericService<Response, Request, PrimaryKeyType>
{
    Task<IEnumerable<Response>> GetAll();

    Task<Response?> Get(PrimaryKeyType id);

    Task Add(Request entity);

    Task Update(PrimaryKeyType id, Request entity);

    Task Delete(PrimaryKeyType id);
}
