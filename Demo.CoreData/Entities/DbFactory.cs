using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Entities;

public class DbFactory : IDisposable
{
    private readonly Func<DemoDbContext> _instanceFunc;
    private DbContext _dbContext;
    private bool _disposed;

    public DbFactory(Func<DemoDbContext> dbContextFactory)
    {
        _instanceFunc = dbContextFactory;
    }

    public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

    public void Dispose()
    {
        if (!_disposed && _dbContext != null)
        {
            _disposed = true;
            _dbContext.Dispose();
        }
    }
}