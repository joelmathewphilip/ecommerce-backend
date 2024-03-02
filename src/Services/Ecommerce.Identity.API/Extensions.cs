using Ecommerce.Identity.API.Dto;

namespace Ecommerce.Identity.API
{
    public static class Extensions
    {
        public static UserIdentityDto AsDto(this UserIdentity userIdentity)
        {
            return
                new UserIdentityDto()
                {
                    userid = userIdentity.userid,
                    username = userIdentity.username,
                };
        }
    }
}
