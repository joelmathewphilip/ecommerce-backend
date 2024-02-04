using Ecommerce.Account.API.Controllers;
using Ecommerce.Account.API.Interfaces;
using MassTransit;
using MassTransit.Transports;
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
        private IPublishEndpoint publishEndpoint;

        public AccountAPIUnitTests()
        {
            _logger = new LoggerFactory().CreateLogger<AccountController>();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json",optional:true)
                .AddEnvironmentVariables()
                .Build();
            _repository = Substitute.For<IUserRepository>();
            //publishEndpoint = new PublishEndpoint();
            //accountController = new AccountController(_repository, _logger);
        }
    }
}
