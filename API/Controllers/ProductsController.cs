using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]

public class ProductsController(IProducrRepository repo) : ControllerBase
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
        return Ok(await repo.GetProductsAsync( brand, type,sort));
    }
    [HttpGet("{id:int}")]
    // [HttpGet]
    // [Route("{id:int}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
    {
        // var products = await context.Products.ToListAsync();
        // var product = await context.Products.FindAsync(id);
        // return product != null ? Ok(product) : NotFound();
        var product = await repo.GetProductByIdAsync(id);
        return product != null ? Ok(product) : NotFound();
    }
    [HttpPost]
    public async Task<ActionResult<IEnumerable<Product>>> CreateProduct(Product product)
    {
        // context.Products.Add(product);
        // await context.SaveChangesAsync();
        // return Ok(product);
        repo.AddProduct(product);
        if (await repo.SaveChangesAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        return BadRequest("Problem Createing Product");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !IsProductExists(id)) return BadRequest("Cannot update this product");
        // context.Entry(product).State = EntityState.Modified;
        // await context.SaveChangesAsync();
        repo.UpdateProduct(product);
        if (await repo.SaveChangesAsync())
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
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        repo.DeleteProduct(product);

        if (await repo.SaveChangesAsync())
            return NoContent();
        return BadRequest("Problem deleteing the product");
        // return NoContent();
    }





    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }


    private bool IsProductExists(int id)
    {
        return repo.IsProductExists(id);
        // return context.Products.Any(x => x.Id == id);
    }

}
