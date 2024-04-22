using System.Linq.Expressions;

namespace PoLoAnalysisAuthServer.Core.Services;

public interface IGenericService<TEntity> where TEntity:class
{
    
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);

    Task Update(TEntity? entity);
    Task Remove(TEntity entity);
}