﻿namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public T? Find(Func<T, bool> predicate);

        public Task<IEnumerable<T>> FindAll(Func<T, bool> predicate);

        public Task CreateAsync(T item);
        
        public Task UpdateAsync(T item);

        public Task DeleteAsync(T item);
    }
}
