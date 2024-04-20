using PoLoAnalysisBusiness.Core.UnitOfWorks;

namespace PoLoAnalysisBusiness.Repository.UnitOfWorks;

public class UnitOfWork:IUnitOfWork
{
    private readonly AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
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