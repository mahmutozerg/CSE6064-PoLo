using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;

namespace PoLoAnalysisBusiness.Services.Services;

public class UserService:GenericService<User>,IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUserRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    {
        _userRepository = repository;
        _unitOfWork = unitOfWork;
    }
}