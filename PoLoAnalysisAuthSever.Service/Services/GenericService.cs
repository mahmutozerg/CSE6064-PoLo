using System.Linq.Expressions;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.Repositories;
using PoLoAnalysisAuthServer.Core.Services;

namespace PoLoAnalysisAuthSever.Service.Services;

public class GenericService<T> : IGenericService<T> where T :class
{
    private readonly IGenericRepository<T?> _repository;
    private readonly IUnitOfWork _unitOfWork;
    public GenericService(IGenericRepository<T?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public IQueryable<T?> Where(Expression<Func<T?, bool>> expression)
    {
        var entities = _repository.Where(expression);
        return entities;
    }
    public async Task Update(T? entity)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task Remove(T entity)
    {
        _repository.Remove(entity);
        await _unitOfWork.CommitAsync();
    }

 
}