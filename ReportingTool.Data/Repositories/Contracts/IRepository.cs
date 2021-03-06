﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ReportingTool.Data.Repositories.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetAsync(int primaryKey);

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(int primaryKey);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities);

        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}
