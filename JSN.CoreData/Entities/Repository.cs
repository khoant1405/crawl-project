using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace JSN.CoreData.Entities;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbFactory _dbFactory;
    private DbSet<T> _dbSet;

    public Repository(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    protected DbSet<T> DbSet => _dbSet ??= _dbFactory.DbContext.Set<T>();

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public void AddRange(List<T> entity)
    {
        DbSet.AddRange(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public void DeleteRange(List<T> entity)
    {
        DbSet.RemoveRange(entity);
    }

    public IQueryable<T> List(Expression<Func<T, bool>>? expression)
    {
        if (expression == null) return DbSet;
        return DbSet.Where(expression);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }
}