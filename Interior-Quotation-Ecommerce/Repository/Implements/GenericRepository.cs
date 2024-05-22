using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Interior_Quotation_Ecommerce.Repository.Implements
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;
        public GenericRepository(ApplicationDbContext context) {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);   
            }
            foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if(context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }


        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
