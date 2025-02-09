using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string ? brand,string? type,string? sort);
    Task<Product?> GetProductByIdAsync(int id);
    // Task<IReadOnlyList<string>> GetAllProductByIdAsync(int id);
    //no interact with database so its synchronous
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetTypesAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool IsProductExists(int id);
    Task<bool> SaveChangesAsync();
}
