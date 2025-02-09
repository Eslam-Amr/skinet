using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
        // throw new NotImplementedException();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {var query = context.Set<T>().AsQueryable();
    query = spec.ApplyCriteria(query);
        return await query.CountAsync();
        // throw new NotImplementedException();
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(e => e.Id == id);

        // throw new NotImplementedException();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        // var item =await context.Set<T>().FindAsync(id); 
        // return item ==null ? null : item;
        return await context.Set<T>().FindAsync(id);

        // throw new NotImplementedException();
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        // throw new NotImplementedException();
        return await ApplySpecification(spec).FirstOrDefaultAsync();

    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();

        // throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<T>?> ListAllAsync()
    {

        return await context.Set<T>().ToListAsync();

        // throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        // throw new NotImplementedException();
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        // throw new NotImplementedException();
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);

        // throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
        // throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        // throw new NotImplementedException();
    }
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(),spec);
    }
    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T,TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T,TResult>(context.Set<T>().AsQueryable(),spec);
    }
}
