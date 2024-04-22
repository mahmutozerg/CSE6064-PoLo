using Microsoft.EntityFrameworkCore;
using PoLoAnalysisAuthServer.Core.DTOs;

namespace PoLoAnalysisAuthServer.Repository.Repositories;

public class UnitOfWork:IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

}