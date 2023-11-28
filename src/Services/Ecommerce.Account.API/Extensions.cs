using Ecommerce.Account.API.Model;

namespace Ecommerce.Account.API
{
    public static class Extensions
    {
        internal static UserInsertDto AsUserInsertDto(this User user)
        {
            return new UserInsertDto()
            {
                Cart = new CartUserInsertDto()
                {
                    CartId = user.Cart.CartId,
                    CartTotalCost = user.Cart.CartTotalCost,
                    CartTotalQuantity = user.Cart.CartTotalQuantity
                },
                Address = user.Address,
                DefaultPaymentMode = user.DefaultPaymentMode,
                Email = user.Email,
                Id = user.Id,
                Mobile = user.Mobile,
                Name = user.Name,
                Orders = user.Orders ?? new List<Guid>(),
            };
        }
        internal static UserUpdateDto AsUserUpdateDto(this User user)
        {
            return new UserUpdateDto()
            {
                Cart = new CartUserUpdateDto()
                {
                    CartId = user.Cart.CartId,
                    CartTotalCost = user.Cart.CartTotalCost,
                    CartTotalQuantity = user.Cart.CartTotalQuantity
                },
                Address = user.Address,
                DefaultPaymentMode = user.DefaultPaymentMode,
                Email = user.Email,
                Mobile = user.Mobile,
                Name = user.Name,
                Orders = user.Orders
            };
        }
        internal static User AsUser(this UserInsertDto userInsertDto)
        {
            return new User()
            {
                Address = userInsertDto.Address,
                Cart = userInsertDto.Cart == null ? new UserCart()
                {
                    CartId = Guid.NewGuid(),
                    CartTotalCost = 0.0,
                    CartTotalQuantity = 0
                } : new UserCart()
                {
                    CartId = userInsertDto.Cart.CartId,
                    CartTotalCost = userInsertDto.Cart.CartTotalCost,
                    CartTotalQuantity = userInsertDto.Cart.CartTotalQuantity
                },
                DateOfCreation = DateTime.Now,
                DefaultPaymentMode = userInsertDto.DefaultPaymentMode,
                Email = userInsertDto.Email,
                Id = Guid.NewGuid(),
                Mobile = userInsertDto.Mobile,
                Name = userInsertDto.Name,
                Orders = userInsertDto.Orders == null ? new List<Guid>() : userInsertDto.Orders
            };
        }

        internal static User AsUser(this UserUpdateDto userUpdateDto, User ExistingUser)
        {
            return new User()
            {
                Address = userUpdateDto.Address ?? ExistingUser.Address,
                Cart = (userUpdateDto.Cart != null ? new UserCart()
                {
                    CartId = userUpdateDto.Cart.CartId,
                    CartTotalCost = userUpdateDto.Cart.CartTotalCost,
                    CartTotalQuantity = userUpdateDto.Cart.CartTotalQuantity
                } : ExistingUser.Cart),
                DefaultPaymentMode = userUpdateDto.DefaultPaymentMode ?? ExistingUser.DefaultPaymentMode,
                Email = userUpdateDto.Email ?? ExistingUser.Email,
                Id = ExistingUser.Id,
                DateOfCreation = ExistingUser.DateOfCreation,
                Mobile = userUpdateDto.Mobile ?? ExistingUser.Mobile,
                Name = userUpdateDto.Name ?? ExistingUser.Name,
                Orders = userUpdateDto.Orders ?? ExistingUser.Orders
            };
        }
    }
}
