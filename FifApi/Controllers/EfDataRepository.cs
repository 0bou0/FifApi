using FifApi.Models;
using FifApi.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class EfDataRepository<T> : IDataRepository<T> where T : class
{
    private readonly FifaDBContext _context;

    public EfDataRepository(FifaDBContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(int id, T entity)
    {
        var existingEntity = await _context.Set<T>().FindAsync(id);
        if (existingEntity == null)
            return null;

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return existingEntity;
    }

    public async Task<T> DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
            return null;

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }


    public IQueryable<T> GetAll()
    {
        return _context.Set<T>().AsQueryable();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression).AsQueryable();
    }

    public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
    {
        return _context.Set<T>().Select(selector);
    }

    public IEnumerable<T> GetAllAsEnumerable()
    {
        return _context.Set<T>().AsEnumerable();
    }


}
