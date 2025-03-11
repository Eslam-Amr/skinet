// // using Core.Interfaces;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using StackExchange.Redis;
// using System.Threading.Tasks;
// using Core.Entities;
// using System.Text.Json; 
// namespace Infrastructure.Services
// {
//     public class CartService(IConnectionMultiplexer redis) : ICartService
//     {
//         private readonly IDatabase _db=redis.GetDatabase();
//         public async Task<ShoppingCart?> GetCartAsync(string key){
//             var data = await _db.StringGetAsync(key);
//             return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
//         }
//         public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart){
//             var created = await _db.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart));
//             // return created ? cart : null;
//             if(!created) return null;
//             return await GetCartAsync(cart.Id);
//         }
//         public async Task<bool> DeleteCartAsync(string key){
//             return await _db.KeyDeleteAsync(key);
//         }
//     }
// }



using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class CartService(IConnectionMultiplexer redis) : ICartService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteCartAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        var data = await _database.StringGetAsync(key);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var created = await _database.StringSetAsync(cart.Id, 
            JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
    
        if (!created) return null;

        return await GetCartAsync(cart.Id);
    }
}