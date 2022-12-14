namespace Demo.CoreData.Common
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
