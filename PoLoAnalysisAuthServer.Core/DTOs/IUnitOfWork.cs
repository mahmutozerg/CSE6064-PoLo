namespace PoLoAnalysisAuthServer.Core.DTOs;

public interface IUnitOfWork
{
    Task CommitAsync();
    void Commit();
}