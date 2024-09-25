namespace Forum.Core.Repositories.Interfaces;

public interface IGenericRepository<Type, PrimaryKeyType> where Type : class
{
    Task<IEnumerable<Type>> GetAllAsync();

    Task<Type> GetByIdAsync(PrimaryKeyType id);

    Task AddAsync(Type entity);

    Task UpdateAsync(Type entity);

    Task UpdateAsync(PrimaryKeyType key, Type value);

    Task DeleteAsync(PrimaryKeyType id);
}
