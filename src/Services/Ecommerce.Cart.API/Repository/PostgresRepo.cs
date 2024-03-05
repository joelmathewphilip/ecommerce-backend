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
                    string insertQuery = "INSERT INTO CartItems (CartId, ItemId, ItemQuantity) VALUES (@CartId, @ItemId, @ItemQuantity)";
                    return await _dbConnection.ExecuteAsync(insertQuery, new { CartId = cartId, ItemId = cartItem.itemid, ItemQuantity = cartItem.itemquantity});
                }
                else
                {
                    string updateQuery = "UPDATE CartItems SET ItemQuantity = @ItemQuantity WHERE CartId = @CartId AND ItemId = @ItemId";
                    Object parameters;
                    if (cartItem.itemquantity == 0)
                    {
                        parameters = new { ItemQuantity = existingCartItem.itemquantity + 1, CartId = cartId, ItemId = cartItem.itemid };
                }
                    else
                    {
                        parameters = new { ItemQuantity = cartItem.itemquantity, CartId = cartId, ItemId = cartItem.itemid };
                    }
                    return await _dbConnection.ExecuteAsync(updateQuery, parameters);
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

        public async Task<int> CartExists(string cartId)
        {
            try
            {
                string fetchCart = "select * from UserCart where Cartid = @CartId";
                var result = await _dbConnection.QueryAsync(fetchCart, new { @CartId = cartId });
                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not fetch cart:" + ex.ToString());
                throw;
            }
        }

        public async Task<dynamic> GetCartQuantity(string cartId)
        {
            try
            {
                var totalItems = 0;
                double totalCost = 0;
                string sql = "SELECT * FROM CartItems WHERE cartId = @CartId";
                var parameter = new { CartId = cartId };
                var response = await _dbConnection.QueryAsync<CartItem>(sql, parameter);
                totalItems = response.Sum(item => item.itemquantity);
                return new  CartCount{ cartCount = totalItems, CartId = Guid.Parse(cartId) };
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to fetch cart details: " + ex.ToString());
                throw;
            }

        }
        public async Task<CartEntity> GetCartAll(string cartId)
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
                        })
                        .ToList(),
                    noOfItems = response.Sum(item => item.itemquantity),
                };
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while fetching details from Cart", ex);
                throw new Exception("Error fetching details from CartDb");
            }

        }
    }

}
