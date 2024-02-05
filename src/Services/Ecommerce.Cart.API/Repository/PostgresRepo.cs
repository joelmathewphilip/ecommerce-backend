using Dapper;
using Ecommerce.Cart.API.Entity;
using System.Data;

namespace Ecommerce.Cart.API.Repository
{

    public class PostgresRepo : ICartRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<PostgresRepo> _logger;
        public PostgresRepo(IDbConnection dbConnection, ILogger<PostgresRepo> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public async Task<int> CreateCart(string CartId, string UserId)
        {
            try
            {
                string insertQuery = "INSERT INTO UserCart  (UserId, CartId) VALUES (@UserId, @CartId)";
                return await _dbConnection.ExecuteAsync(insertQuery, new { UserId, CartId });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert cart data", ex);
                throw;
            }
        }
        public async Task<dynamic> AddItem(CartItem cartItem, string cartId)
        {
            try
            {
                string selectQuery = "SELECT * FROM CartItems WHERE CartId = @CartId AND ItemId = @ItemId";
                var existingCartItem = await _dbConnection.QuerySingleOrDefaultAsync<CartItem>(selectQuery, new { CartId = cartId, ItemId = cartItem.itemid });

                if (existingCartItem == null)
                {
                    string insertQuery = "INSERT INTO CartItems (CartId, ItemId, ItemQuantity, ItemCost) VALUES (@CartId, @ItemId, @ItemQuantity, @ItemCost)";
                    return await _dbConnection.ExecuteAsync(insertQuery, new {CartId = cartId, ItemId = cartItem.itemid, ItemQuantity = cartItem.itemquantity, ItemCost = cartItem.itemcost });
                }
                else
                {
                    string updateQuery = "UPDATE CartItems SET ItemQuantity = @ItemQuantity, ItemCost = @ItemCost WHERE CartId = @CartId AND ItemId = @ItemId";
                    return await _dbConnection.ExecuteAsync(updateQuery, new { ItemQuantity = existingCartItem.itemquantity + cartItem.itemquantity, ItemCost = cartItem.itemcost, CartId = cartId, ItemId = cartItem.itemid });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add item to cart", ex);
                throw;
            }
        }


        public async Task<int> DeleteItem(string cartid, string itemId)
        {
            try
            {
                string sql = "Delete from CartItems where CartId = @CartId and ItemId = @ItemId";
                var parameters = new { CartId = cartid, ItemId = itemId };
                return await _dbConnection.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete item from cart", ex.Message);
                throw;
            }
        }

        /*public async Task<IEnumerable<CartEntity>> GetAllCarts()
        {
            try
            {
                using (var _dbConnection = _db_dbConnectionection.Create_dbConnectionection())
                {
                    string sql = "select * from CartItems";
                    var cartEntity = await _dbConnection.QueryAsync<CartEntity>(sql);
                    return cartEntity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get all carts", ex.Message);
                return null;
            }
        }*/

        public async Task<int> CartExists(string cartId)
        {
            try
            {
                string fetchCart = "select * from UserCart where Cartid = @CartId";
                var result =  await _dbConnection.QueryAsync(fetchCart, new { @CartId = cartId });
                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not fetch cart");
                throw;
            }
        }
        public async Task<CartEntity> GetCart(string cartId)
        {
            try
            {
                string sql = "SELECT * FROM CartItems WHERE cartId = @CartId";
                var parameters = new { CartId = cartId };
                var response = await _dbConnection.QueryAsync<CartItem>(sql, parameters);

                var cart = new CartEntity
                {
                    Id = Guid.Parse(cartId),
                    cartItems = response
                        .GroupBy(item => item.itemid)
                        .Select(group => new CartItem
                        {
                            itemid = group.Key,
                            itemquantity = group.Sum(item => item.itemquantity),
                            itemcost = group.First().itemcost
                        })
                        .ToList(),
                    noOfItems = response.Sum(item => item.itemquantity),
                    totalCost = response.Sum(item =>
                    {
                        return item.itemquantity * item.itemcost;
                    })
                };
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while fetching details from Cart", ex);
                throw;
            }

        }
    }

}
