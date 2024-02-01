using Ecommerce.Account.API.Controllers;
using Ecommerce.Account.API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Ecommerce.UnitTests
{
    public class AccountAPIUnitTests
    {
        private IUserRepository _repository;
        private AccountController accountController;
        private ILogger<AccountController> _logger;
        private IConfiguration _configuration;

        public AccountAPIUnitTests()
        {
            _logger = new LoggerFactory().CreateLogger<AccountController>();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json",optional:true)
                .AddEnvironmentVariables()
                .Build();
            _repository = Substitute.For<IUserRepository>();
            accountController = new AccountController(_repository, _logger);
        }
    }
}
