using System.Linq.Expressions;

namespace Demo.CoreData.Entities;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    void AddRange(List<T> entity);
    void Delete(T entity);
    void Update(T entity);
    IQueryable<T> List(Expression<Func<T, bool>> expression);
}