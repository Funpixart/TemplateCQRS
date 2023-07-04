using System.Linq.Expressions;

namespace TemplateCQRS.Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    /// <summary>
    ///     Asynchronously retrieves all entities of type T from the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous retrieval operation, with an enumerable collection of entities of type T.</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves all entities of type T that satisfy the specified predicate from the data store.
    /// </summary>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A task that represents the asynchronous retrieval operation,
    ///     with an enumerable collection of entities of type T that satisfy the predicate. </returns>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves an entity of type T with the specified id from the data store.
    /// </summary>
    /// <param name="id">The id of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous retrieval operation, with the retrieved entity of type T or null if not found.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    ///     Asynchronously creates and persists a new entity of type T in the data store.
    /// </summary>
    /// <param name="entity">The entity to create and persist.</param>
    /// <returns>A task that represents the asynchronous creation operation, with the created entity of type T.</returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    ///     Asynchronously updates and persists an existing entity of type T in the data store.
    /// </summary>
    /// <param name="entity">The entity to update and persist.</param>
    /// <returns>A task that represents the asynchronous update operation, with the updated entity of type T.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    ///     Asynchronously deletes an entity of type T with the specified id from the data store.
    /// </summary>
    /// <param name="id">The id of the entity to delete.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves all entities of type T that satisfy the specified predicate from the data store.
    /// </summary>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A task that represents the asynchronous retrieval operation,
    ///     with an enumerable collection of entities of type T that satisfy the predicate. </returns>
    Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously checks whether any entities of type T satisfy the specified predicate in the data store,
    ///     and cancels the operation if the token is cancelled.
    /// </summary>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation,
    ///     returning true if any entities satisfy the predicate, and false otherwise.
    /// </returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously gets the total count of entities of type T in the data store.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation,
    ///     with the total count of entities of type T.</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously gets the count of entities of type T that satisfy the specified predicate in the data store.
    /// </summary>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, with the count of
    ///     entities of type T that satisfy the predicate.</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves a paged collection of entities of type T from the data store.
    /// </summary>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A task that represents the asynchronous retrieval operation,
    ///     with an enumerable collection of paged entities of type T.</returns>
    Task<IEnumerable<T>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves a paged collection of entities of type T that satisfy the specified predicate from the data store.
    /// </summary>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A task that represents the asynchronous retrieval operation,
    ///     ith an enumerable collection of paged entities of type T
    ///     that satisfy the predicate.</returns>
    Task<IEnumerable<T>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Provides a queryable interface for the entities of type T in the data store.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    ///     An IQueryable interface that can be used to compose and execute
    ///     queries against the entities.</returns>
    IQueryable<T> AsQueryable(CancellationToken cancellationToken = default);
}