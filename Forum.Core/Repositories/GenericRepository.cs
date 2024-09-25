using Forum.Core.Repositories.Interfaces;
using Forum.EF;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Repositories;

public abstract class GenericRepository<Type, PrimaryKeyType> : IGenericRepository<Type, PrimaryKeyType> where Type : class
{
    protected ForumContext _context;
    protected DbSet<Type> _dbSet;

    protected GenericRepository(ForumContext context)
    {
        _context = context;
        _dbSet = _context.Set<Type>();
    }

    public async Task AddAsync(Type entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PrimaryKeyType id)
    {
        Type entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            _dbSet.Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Type>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Type> GetByIdAsync(PrimaryKeyType id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task UpdateAsync(PrimaryKeyType key, Type entity)
    {
        var entityToUpdate = _dbSet.Find(key);
        _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Type entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
