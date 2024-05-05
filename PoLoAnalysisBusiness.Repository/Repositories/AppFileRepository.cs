using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using File = SharedLibrary.Models.business.File;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class AppFileRepository:GenericRepository<File>,IAppFileRepository
{
    private readonly DbSet<File> _files;
    public AppFileRepository(AppDbContext context) : base(context)
    {
        _files = context.Set<File>();
    }
}