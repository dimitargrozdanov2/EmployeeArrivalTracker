using Microsoft.EntityFrameworkCore;
using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;
using ReportingTool.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ReportingTool.Data.Repositories
{
    public class DbRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDbContext dbContext;

        public DbRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            ObjectCheck<TEntity>.EntityCheck(entity, $"{nameof(TEntity)} missing.");
            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            ObjectCheck<TEntity>.PrimaryKeyCheck(id, $"primaryKey <= 0 in {nameof(IRepository<TEntity>)}");
            return await dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return (filter != null ? await dbContext.Set<TEntity>().Where(filter).ToListAsync() : await dbContext.Set<TEntity>().ToListAsync());
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await dbContext.Set<TEntity>().SingleOrDefaultAsync(filter);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            ObjectCheck<TEntity>.EntityCheck(entity, $"{nameof(TEntity)} missing.");
            dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entityToBeDeleted = await GetAsync(id);
            ObjectCheck<TEntity>.EntityCheck(entityToBeDeleted, $"{nameof(TEntity)} missing.");

            dbContext.Set<TEntity>().Remove(entityToBeDeleted);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().AddRange(entities);
            await dbContext.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().UpdateRange(entities);
            await dbContext.SaveChangesAsync();
            return entities;
        }
    }
}
