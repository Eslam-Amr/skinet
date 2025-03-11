using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CartService(IConnectionMultiplexer redis) : ICartService
    {
        private readonly IDatabase _db=redis.GetDatabase();
        public async Task<ShoppingCart?> GetCartAsync(string key){
            var data = await _db.StringGetAsync(key);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
        }
        public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart){
            // var created = await _db.StringSetAsync(cart.Id, JsonConvert.SerializeObject(cart));
            // return created ? cart : null;
        }
        public async Task<bool> DeleteCartAsync(string key){
            return await _db.KeyDeleteAsync(key);
        }
    }
}
