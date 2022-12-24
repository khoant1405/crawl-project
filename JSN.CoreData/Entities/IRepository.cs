using System.Linq.Expressions;

namespace JSN.CoreData.Entities;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    void AddRange(List<T> entity);
    void Delete(T entity);
    void DeleteRange(List<T> entity);
    void Update(T entity);
    IQueryable<T> List(Expression<Func<T, bool>> expression);
}