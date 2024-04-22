using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class UserRepository:GenericRepository<AppUser>,IUserRepository
{
    private readonly DbSet<AppUser> _users;
    public UserRepository(AppDbContext context) : base(context)
    {
        _users = context.Set<AppUser>();
    }
}