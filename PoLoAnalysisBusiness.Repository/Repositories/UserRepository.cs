using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;

namespace PoLoAnalysisBusiness.Repository.Repositories;

public class UserRepository:GenericRepository<User>,IUserRepository
{
    private readonly DbSet<User> _users;
    public UserRepository(AppDbContext context) : base(context)
    {
        _users = context.Set<User>();
    }
}