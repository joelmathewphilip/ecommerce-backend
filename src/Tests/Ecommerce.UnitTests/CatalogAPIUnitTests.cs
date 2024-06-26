﻿using Ecommerce.Catalog.API.Controllers;
using Ecommerce.Catalog.API.Interfaces;
using Ecommerce.Catalog.API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Ecommerce.UnitTests
{
    public class CatalogAPIUnitTests
    {
        private ICatalogItemRepository _repository;
        private CatalogItemsController _catalogItemsController;
        private ILogger<CatalogItemsController> _logger;
        private IConfiguration _configuration;
        private IDiscountService _discountService;
        public CatalogAPIUnitTests()
        {
            _logger = new LoggerFactory().CreateLogger<CatalogItemsController>();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables().
                Build();
            _repository = Substitute.For<ICatalogItemRepository>();
        }

        [Fact]
        public async Task GetItemAsync_WithNoExistingItem_ReturnsNotFound()
        {
            //Arrange
            _repository.GetCatalogItemAsync(Guid.NewGuid()).ReturnsNull();
            
            _discountService = Substitute.For<IDiscountService>();
            var catalogItem = CreateRandomCatalogItem();
            _discountService.FetchDiscountedPrice(catalogItem).Returns(catalogItem.CatalogMrp - (20 / 100 * catalogItem.CatalogMrp));
            _catalogItemsController = new CatalogItemsController(_repository, _logger, _configuration, _discountService);

            //Act
            var result = ((NotFoundResult)await _catalogItemsController.GetCatalogItemAsync(Guid.NewGuid()));

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAync_ExistingItem_ReturnsItem()
        {
            //Arrange
            var catalogItem = CreateRandomCatalogItem();
            _repository.GetCatalogItemAsync(catalogItem.CatalogId).Returns(catalogItem);
            _discountService = Substitute.For<IDiscountService>();
            _discountService.FetchDiscountedPrice(catalogItem).Returns(catalogItem.CatalogMrp - (20 / 100 * catalogItem.CatalogMrp));
            _catalogItemsController = new CatalogItemsController(_repository, _logger, _configuration, _discountService);

            //Act
            var result = ((ObjectResult)await _catalogItemsController.GetCatalogItemAsync(catalogItem.CatalogId)).Value;


            //Assert
            result.Should().BeEquivalentTo(catalogItem, options => options.ComparingByMembers<CatalogItem>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetItemsAync_WithExistingItems_ReturnsAllItems()
        {
            //Arrange
            var catalogItems = CreateListOfRandomItems();
            _repository.GetCatalogItemsAsync().Returns(catalogItems);
            _discountService = Substitute.For<IDiscountService>();
            var catalogItem = CreateRandomCatalogItem();
            _discountService.FetchDiscountedPrice(catalogItem).Returns(catalogItem.CatalogMrp - (20/100* catalogItem.CatalogMrp));
            _catalogItemsController = new CatalogItemsController(_repository, _logger, _configuration, _discountService);

            //Act
            var result = ((ObjectResult)await _catalogItemsController.GetCatalogItemsAsync()).Value;

            //Assert
            result.Should().BeEquivalentTo(catalogItems, options => options.ComparingByMembers<CatalogItem>().ExcludingMissingMembers());
        }



        private IEnumerable<CatalogItem> CreateListOfRandomItems()
        {
            return new List<CatalogItem>()
            {
                CreateRandomCatalogItem(),
                CreateRandomCatalogItem(),
                CreateRandomCatalogItem()
            };
        }
        private CatalogItem CreateRandomCatalogItem()
        {
            return new CatalogItem()
            {
                CatalogId = Guid.NewGuid(),
                CatalogDateOfCreation = DateTime.Now,
                CatalogDescription = "random description",
                CatalogImages = new List<string>()
                {
                    new string("123"),
                    new string("123"),
                },
                CatalogMrp = new Random().Next(1000),
                DiscountedPrice = new Random().Next(1000),
                CatalogName = "random name",
                CatalogType = "random type"
            };
        }
    }
}
