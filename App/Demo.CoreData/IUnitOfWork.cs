using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Demo.CoreData
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
