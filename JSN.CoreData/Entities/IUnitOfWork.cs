namespace JSN.CoreData.Entities;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}