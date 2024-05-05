using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class ResultRepository:GenericRepository<Result>,IResultRepository
{
    private readonly DbSet<Result> _results;
    public ResultRepository(AppDbContext context) : base(context)
    {
        _results = context.Set<Result>();
    }
}