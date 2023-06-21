namespace TemplateCQRS.Infrastructure.Data;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Retrieves all entities of the specified type as an `IQueryable` for further querying.
    /// </summary>
    IQueryable<TEntity> ReadAll<TEntity>() where TEntity : class;

    /// <summary>
    /// Adds the specified entity to the context underlying the set in the 'Added' state such that it will be inserted into the database when `SaveChangesAsync` is called.
    /// </summary>
    Task AddAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity in the 'Modified' state such that it will be updated in the database when `SaveChangesAsync` is called.
    /// </summary>
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity in the 'Deleted' state such that it will be removed from the database when `SaveChangesAsync` is called.
    /// </summary>
    Task RemoveAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Finds and returns an entity of type TEntity from the data store by its key value(s).
    /// If the entity is not found, returns null.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to find.</typeparam>
    /// <param name="key">The key value(s) of the entity to find.</param>
    /// <returns>A task that represents the asynchronous operation, with a result of the found entity, or null if not found.</returns>
    Task<TEntity?> FindByKey<TEntity>(params object?[]? key) where TEntity : class;
}