using System.Linq.Expressions;
namespace Entities.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        //_context.Categories.Include("Products").ToList();
        //_context.Categories.Where(x=>x.Id == Id).ToList();
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);
        //_context.Categories.Include("Products").SinglOrDefault();
        //_context.Categories.Where(x=>x.Id == Id).SinglOrDefault();
        T GetFirstorDefault(Expression<Func<T, bool>>? perdicate = null, string? IncludeWord = null);
        //_context.Categories.Add(category);
        void Add(T entity);
        //_context.Categories.Remove(category);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
