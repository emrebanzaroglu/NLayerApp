using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        // productRepository.Where(x=>x.id>5).OrderBy.ToLis();  // ıqueryable yaparak tolist diyene kadar veritabanına gitmez, tüm sorgular yazılıp tolist yapılınca gider ve daha performanslı çalışır.
        IQueryable<T> Where(Expression<Func<T,bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T,bool>> expression);  //var mı yok mu kontrolü yapacak
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);  //update ve remove'un asenkron metodları yok. ef core tarında karşılıkları yok asenkron olarak çünkü uzun süren bir işlem değil ancak add uzun süren ir işlem çünkü veritabanına gidiyor. Diğerleri memoryden çekiyor
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
