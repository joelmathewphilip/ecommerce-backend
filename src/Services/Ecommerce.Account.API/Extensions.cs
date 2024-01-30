using Ecommerce.Account.API.Model;

namespace Ecommerce.Account.API
{
    public static class Extensions
    {
        internal static UserInsertDto AsUserInsertDto(this User user)
        {
            return new UserInsertDto()
            {
               CartId = user.CartId,
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
                CartId = user.CartId,
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
                CartId = new Guid(),
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
                CartId = ExistingUser.CartId,
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
