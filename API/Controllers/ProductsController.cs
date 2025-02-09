using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]

// public class ProductsController(IProductRepository repo) : ControllerBase
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    // private readonly StoreContext context;

    // public ProductsController(StoreContext context)
    // {
    //     this.context = context;
    // }
    [HttpGet]
    [Route("")]
    //     public  Task<ActionResult<IEnumerable<Product>>> Get()
    //     // public async Task<ActionResult<IEnumerable<Product>>> Get()
    //     {
    // return Ok(context.Products.ToList());
    //     }
    // public async Task<ActionResult<IEnumerable<Product>>> Get()
    public async Task<ActionResult<IReadOnlyList<Product>>> Get(//[FromQuery]by default
    string? brand,
    string? type,string? sort)
    {
        // var products = await context.Products.ToListAsync();
        // return Ok(products);
        // return Ok(await repo.GetProductsAsync( brand, type,sort));
        var spec = new ProductSpecification(brand,type,sort);
        var products = await repo.ListAsync(spec);
        return Ok(products);
    }
    [HttpGet("{id:int}")]
    // [HttpGet]
    // [Route("{id:int}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
    {
        // var products = await context.Products.ToListAsync();
        // var product = await context.Products.FindAsync(id);
        // return product != null ? Ok(product) : NotFound();
        // var product = await repo.GetProductByIdAsync(id);
        var product = await repo.GetByIdAsync(id);
        return product != null ? Ok(product) : NotFound();
    }
    [HttpPost]
    public async Task<ActionResult<IEnumerable<Product>>> CreateProduct(Product product)
    {
        // context.Products.Add(product);
        // await context.SaveChangesAsync();
        // return Ok(product);
        // repo.AddProduct(product);
        // if (await repo.SaveChangesAsync())
        repo.Add(product);
        if (await repo.SaveAllAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        return BadRequest("Problem Createing Product");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !IsProductExists(id)) return BadRequest("Cannot update this product");
        // context.Entry(product).State = EntityState.Modified;
        // await context.SaveChangesAsync();
        // repo.UpdateProduct(product);
        // if (await repo.SaveChangesAsync())
        repo.Update(product);
        if (await repo.SaveAllAsync())
            return NoContent();
        return BadRequest("Problem updating the product");

    }

    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        // Console.WriteLine("Delete Product");
        // var product = await context.Products.FindAsync(id);
        // if (product == null) return NotFound();
        // context.Products.Remove(product);
        // await context.SaveChangesAsync();
        // var product = await repo.GetProductByIdAsync(id);
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();
        repo.Remove(product);
        // repo.DeleteProduct(product);

        // if (await repo.SaveChangesAsync())
        if (await repo.SaveAllAsync())
            return NoContent();
        return BadRequest("Problem deleteing the product");
        // return NoContent();
    }





    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        //TODO ----------------------------------------------------------------
        // return Ok(await repo.GetBrandsAsync());
        var spec = new BrandListSpecification();

        return Ok(await repo.ListAsync(spec));
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        //TODO ----------------------------------------------------------------
    var spec = new TypeListSpecification();

        return Ok(await repo.ListAsync(spec));
    
        // return Ok(await repo.GetTypesAsync());
        // return Ok();
    }


    private bool IsProductExists(int id)
    {
        return repo.Exists(id);
        // return repo.IsProductExists(id);
        // return context.Products.Any(x => x.Id == id);
    }

}
