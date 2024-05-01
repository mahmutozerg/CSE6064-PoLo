using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Services.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity :Base
{
    private readonly IGenericRepository<TEntity?> _repository;
    private readonly IUnitOfWork _unitOfWork;


    public GenericService(IGenericRepository<TEntity?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }


    public async Task<CustomResponseNoDataDto> Remove(string id,string updatedBy)
    {
        var entity = await _repository.Where(x => x!.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
        if (entity is null )
            return CustomResponseNoDataDto.Fail(StatusCodes.NotFound,ResponseMessages.NotFound);
        
        entity.UpdatedAt =DateTime.Now;
        entity.UpdatedBy = updatedBy;
        _repository.Remove(entity);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }

    public async Task<CustomResponseDto<TEntity>> AddAsync(TEntity entity,string createdBy)
    {
        var entityExist = await _repository.AnyAsync(x => x != null && x.Id == entity.Id && !x.IsDeleted);
        if (entityExist )
            throw new Exception(ResponseMessages.AlreadyExists);


        entity.UpdatedBy = createdBy;
        entity.CreatedBy = createdBy;
        entity.CreatedAt = DateTime.Now;
        await _repository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        return CustomResponseDto<TEntity>.Success(entity, StatusCodes.Created);
    }

    public IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression)
    {
        var entities = _repository.Where(expression);
        
        return entities;
    }
    
    public async Task<CustomResponseNoDataDto> UpdateAsync(TEntity? entity,string updatedBy)
    {
        if (entity == null) 
            return CustomResponseNoDataDto.Fail(StatusCodes.NotFound,ResponseMessages.NotFound);
        
        entity.UpdatedBy = updatedBy;
        entity.UpdatedAt = DateTime.Now;
        _repository.Update(entity);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(StatusCodes.Updated);

    }

    public async Task<CustomResponseDto<TEntity?>> GetByIdAsync(string id)
    {
        var entity = await _repository.GetById(id);
        if (entity is not null) 
            return  CustomResponseDto<TEntity>.Success(entity,StatusCodes.Ok);

        throw new Exception(ResponseMessages.NotFound);


    }
}