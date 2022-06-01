using System.Linq.Expressions;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        IQueryable<T> FindByConditionWithPaging(Expression<Func<T, bool>> expression, int pageNumber, int pageSize,
            out int totalCount);

        IQueryable<T> FindByConditionWithPagingOrder(Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNumber, int pageSize, out int totalCount);

        IQueryable<T> FindByConditionWithPagingOrder(Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNumber, int pageSize);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        void AddRange(List<T> entities);
        void UpdateRange(List<T> entities);



    }
}
