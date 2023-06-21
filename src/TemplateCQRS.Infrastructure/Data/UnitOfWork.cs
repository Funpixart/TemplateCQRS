using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace TemplateCQRS.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction _transaction = null!;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    private async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public IQueryable<TEntity> ReadAll<TEntity>() where TEntity : class
    {
        return _context.Set<TEntity>();
    }

    public async Task<TEntity?> FindByKey<TEntity>(params object?[]? key) where TEntity : class
    {
        return await _context.Set<TEntity>().FindAsync(key);
    }

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await BeginTransactionAsync();
        _context.Set<TEntity>().Add(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await BeginTransactionAsync();
        _context.Set<TEntity>().Update(entity);
        await SaveChangesAsync();
    }

    public async Task RemoveAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await BeginTransactionAsync();
        _context.Set<TEntity>().Remove(entity);
        await SaveChangesAsync();
    }

    private async Task CommitTransactionAsync()
    {
        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null!;
    }

    private async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null!;
    }

    private async Task<int> SaveChangesAsync()
    {
        try
        {
            var result = await _context.SaveChangesAsync();
            await CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {
            await RollbackTransactionAsync();
            var msg = ex.InnerException is not null ? ex.InnerException.Message : ex.Message;
            Log.Logger.ForContext("Payload", "UnitOfWork")
                .Error("{0}.", msg);
            return 0;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }
}
