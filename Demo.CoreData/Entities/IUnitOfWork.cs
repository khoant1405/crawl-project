namespace Demo.CoreData.Entities;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}