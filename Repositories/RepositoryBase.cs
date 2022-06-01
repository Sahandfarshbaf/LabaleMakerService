using System.Linq.Expressions;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected LabelMaker_BP_DBContext RepositoryContext { get; set; }

        protected RepositoryBase(LabelMaker_BP_DBContext _repositoryContext)
        {
            RepositoryContext = _repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            expression ??= x => true;
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public IQueryable<T> FindByConditionWithPaging(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, out int totalCount)
        {
            expression ??= x => true;

            totalCount = this.RepositoryContext.Set<T>().Where(expression).AsNoTracking().Count();
            return this.RepositoryContext.Set<T>().Where(expression).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking();
        }

        public IQueryable<T> FindByConditionWithPagingOrder(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNumber, int pageSize, out int totalCount)
        {
            totalCount = this.RepositoryContext.Set<T>().Where(expression).AsNoTracking().Count();
            return orderBy(this.RepositoryContext.Set<T>().Where(expression).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking());
        }

        public IQueryable<T> FindByConditionWithPagingOrder(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNumber, int pageSize)
        {

            return orderBy(this.RepositoryContext.Set<T>().Where(expression).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking());
        }


        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {

            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(List<T> entity)
        {
            this.RepositoryContext.Set<T>().RemoveRange(entity);
        }

        public virtual void AddRange(List<T> entities)
        {
            this.RepositoryContext.Set<T>().AddRange(entities);
        }

        public virtual void UpdateRange(List<T> entities)
        {
            this.RepositoryContext.Set<T>().UpdateRange(entities);
        }


    }
}