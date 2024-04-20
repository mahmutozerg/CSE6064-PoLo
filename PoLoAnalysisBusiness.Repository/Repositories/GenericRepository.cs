using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity :Base
{
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }
    
    public IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression)
    {
        var entities =  _dbSet.Where(expression);
        return entities;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }
    
    public void Update(TEntity? entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _dbSet.Update(entity);
    }

    public async Task AddAsync(TEntity? entity)
    {
        entity.CreatedAt = DateTime.Now; 
        entity.UpdatedAt = DateTime.Now; 
        await _dbSet.AddAsync(entity);
    }

    public void Remove(TEntity entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }
}