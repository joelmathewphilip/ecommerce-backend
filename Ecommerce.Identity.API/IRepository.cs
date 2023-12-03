namespace Ecommerce.Identity.API
{
    public interface IRepository
    {
        public Task AddData(UserIdentity userIdentity);

        public Task<UserIdentity> FetchRegisteredUsers(string password);
    }
}
