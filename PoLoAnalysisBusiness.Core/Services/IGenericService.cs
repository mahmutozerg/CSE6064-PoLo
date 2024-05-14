using System.Linq.Expressions;
using SharedLibrary.DTOs.Responses;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IGenericService<TEntity> where TEntity:class
{

    Task<CustomResponseNoDataDto> RemoveAsync(string id,string updatedBy);
    Task<CustomResponseDto<TEntity>> AddAsync(TEntity entity,string createdBy);
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<CustomResponseNoDataDto> UpdateAsync(TEntity? entity,string updatedBy);
    Task<CustomResponseDto<TEntity?>> GetByIdAsync(string id);

}