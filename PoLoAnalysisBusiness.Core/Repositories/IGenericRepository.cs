using System.Linq.Expressions;

namespace PoLoAnalysisBusiness.Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity:class?
{
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> expression);
    void Update(TEntity? entity);
    Task AddAsync(TEntity? entity);
    void Remove(TEntity entity);

    Task<TEntity?> GetById(string id);

}