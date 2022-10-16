using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Shared.Utils.Sql.Contexts;

public class SchoolAppContext : DbContext
{
    public SchoolAppContext(DbContextOptions options) : base(options)
    {

    }

    public virtual void DetachedItem(object item)
    {
        this.Entry(item).State = EntityState.Detached;
    }

    public virtual IQueryable<T> GetQueryable<T>(DbSet<T> dbSet) where T : class
    {
        return dbSet.AsNoTracking();
    }

    public virtual void SetModifiedProperty(object item, string property)
    {
        this.Entry(item).Property(property).IsModified = true;
    }
}
