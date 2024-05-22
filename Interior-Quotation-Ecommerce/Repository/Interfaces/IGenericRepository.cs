using Interior_Quotation_Ecommerce.Entities;
using System.Linq.Expressions;

namespace Interior_Quotation_Ecommerce.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
            );
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
