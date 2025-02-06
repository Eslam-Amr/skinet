using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProducrRepository
{
    public void AddProduct(Product product)
    {
        // throw new NotImplementedException();
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);

        // throw new NotImplementedException();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    // Task<IReadOnlyList<string>> IProducrRepository.GetAllProductByIdAsync(int id)

    {
        return await context.Products.FindAsync(id);
        // throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {

        var query = context.Products.AsQueryable();
        // if()
        query = !string.IsNullOrWhiteSpace(brand) ? query.Where(x => x.Brand == brand) : query;
       
        query = !string.IsNullOrWhiteSpace(type) ? query.Where(x => x.Type == type) : query;
       
    //    if(!string.IsNullOrWhiteSpace(sort)){
        query = sort switch{
            "priceAsc"=>query.OrderBy(x => x.Price),
            "priceDesc"=>query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x=>x.Name)
        };
    //    }
        return await query.ToListAsync();
        // throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context
        .Products
        .Select(x => x.Brand).Distinct().ToListAsync();
        // throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context
      .Products
      .Select(x => x.Type).Distinct().ToListAsync();

        // throw new NotImplementedException();
    }

    public bool IsProductExists(int id)
    {
        return context.Products.Any(p => p.Id == id);
        // throw new NotImplementedException();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
        // throw new NotImplementedException();
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
        // throw new NotImplementedException();
    }

    // Task<IReadOnlyList<string>> IProducrRepository.GetAllProductByIdAsync(int id)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<IReadOnlyList<string>> GetAllProducctByIdAsync(int id)
    // {
    //     throw new NotImplementedException();
    // }
}
