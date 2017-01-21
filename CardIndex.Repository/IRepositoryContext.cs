using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardIndex.Domain.Models;
using System.Data.Entity;

namespace CardIndex.Repository
{
    public interface IRepositoryContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
        DbEntityEntry Entry(object entity);

        Database Database { get; }
    }
}
