using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TemplateCQRS.Infrastructure.Data;

namespace TemplateCQRS.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IUnitOfWork _unitOfWork;

    public Repository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) 
        => await _unitOfWork.ReadAll<T>().ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _unitOfWork.ReadAll<T>().Where(predicate).ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(int id) => await _unitOfWork.FindByKey<T>(id);

    public async Task<T> CreateAsync(T entity)
    {
        await _unitOfWork.AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        await _unitOfWork.UpdateAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
        {
            return;
        }
        await _unitOfWork.RemoveAsync(entity);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        var result = await GetByAsync(x => x == entity, cancellationToken);
        if (result is null)
        {
            return;
        }
        await _unitOfWork.RemoveAsync(result);
    }

    public async Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _unitOfWork.ReadAll<T>().Where(predicate).ToListAsync(cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _unitOfWork.ReadAll<T>().AnyAsync(predicate, cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken) => await _unitOfWork.ReadAll<T>().CountAsync(cancellationToken);

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _unitOfWork.ReadAll<T>().CountAsync(predicate, cancellationToken);

    public async Task<IEnumerable<T>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        => await _unitOfWork.ReadAll<T>().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _unitOfWork.ReadAll<T>().Where(predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

    public IQueryable<T> AsQueryable(CancellationToken cancellationToken = default) => _unitOfWork.ReadAll<T>();
}
