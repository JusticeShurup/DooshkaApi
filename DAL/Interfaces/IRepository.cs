namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();

        public Task<IEnumerable<T>> GetAllByCondition(Func<T, bool> predicate);

        public T Get(int id);
        
        public IEnumerable<T> Find(Func<T, bool> predicate);
        
        public Task<T?> FindByIdAsync(Guid id);

        public Task<T?> FindByEmailAsync(string email);

        public Task CreateAsync(T item);
        
        public Task UpdateAsync(T item);

        public Task DeleteByIdAsync(Guid id);
    }
}
