using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisAuthServer.Core.Repositories;

namespace PoLoAnalysisAuthServer.Repository.Repositories;

public class GenericRepository<T>:IGenericRepository<T> where T :class
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }
    
    public IQueryable<T?> Where(Expression<Func<T?, bool>> expression)
    {
        var entities =  _dbSet.Where(expression);
        return entities;
    }

    public async Task<bool> AnyAsync(Expression<Func<T?, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public async Task AddAsync(T? entity)
    {
         await _dbSet.AddAsync(entity);
    }

    public void Update(T? entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T? entity)
    {
        _dbSet.Remove(entity);
    }


}

