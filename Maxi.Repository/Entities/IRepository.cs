using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Entities
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        Task DeleteAsync(TEntity entity);
        //  IUnitOfWork UnitOfWork { get; }
        Task<TEntity> GetByIdAsync(object id);

        Task ReloadAsync(TEntity entity);
    }
}
