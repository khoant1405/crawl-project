namespace JSN.CoreData.Entities;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbFactory _dbFactory;

    public UnitOfWork(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public Task<int> CommitAsync()
    {
        return _dbFactory.DbContext.SaveChangesAsync();
    }
}