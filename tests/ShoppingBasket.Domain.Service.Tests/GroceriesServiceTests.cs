namespace ShoppingBasket.Domain.Service.Tests;

using ShoppingBasket.Domain.Model;
using ShoppingBasket.Data.Repository;
using Moq;
using AutoFixture;
using ShoppingBasket.Domain.Service.Strategies;
using ShoppingBasket.Domain.Service.Services;
using ShoppingBasket.Domain.Service.Mappers;

public class GroceriesServiceTests
{
    private static Fixture fixture = new Fixture();

    private GroceriesService service;
    private Mock<IGroceryRepository> mockGroceryRepository;
    private Mock<IDiscountStrategyManager> mockDiscountStrategyManager;

    private Mock<IDiscountService> mockDiscountService;

    private Mock<IGroceriesMapper> mockGroceriesMapper;

    [SetUp]
    public void Setup()
    {
        this.mockGroceryRepository = new Mock<IGroceryRepository>();
        this.mockDiscountStrategyManager = new Mock<IDiscountStrategyManager>();
        this.mockDiscountService = new Mock<IDiscountService>();
        this.mockGroceriesMapper = new Mock<IGroceriesMapper>();

        this.service = new GroceriesService(
            this.mockGroceryRepository.Object,
            this.mockDiscountStrategyManager.Object,
            this.mockDiscountService.Object,
            this.mockGroceriesMapper.Object);
    }

    [Test]
    [TestCase("apples,soup", true)]
    [TestCase("", true)]
    public void GroceriesService_BuyGroceries_ShouldCallDependencies(
        string groceriesName,
        bool shouldCallDependencies)
    {
        // Arrange
        var userGroceries = groceriesName.Split(',');
        
        var dependenciesCallTimes = shouldCallDependencies ? Times.Once() : Times.Never();

        this.mockGroceryRepository
            .Setup(s => s.GetGroceriesPrices(userGroceries))
            .Returns(this.GetDataGroceries(userGroceries));

        // Act
        var result = this.service.BuyGroceries(userGroceries);

        // Assert
        this.mockGroceryRepository
            .Verify(s => s.GetGroceriesPrices(userGroceries), Times.Once);

        this.mockGroceriesMapper
            .Verify(s => s.Map(It.IsAny<string[]>(), It.IsAny<List<Data.Model.Grocery>>()), dependenciesCallTimes);

        this.mockDiscountStrategyManager
            .Verify(s => s.GetDiscountItems(It.IsAny<List<Domain.Model.Grocery>>()), dependenciesCallTimes);

        this.mockDiscountService
            .Verify(s => 
                s.GetTotalPrice(
                    It.IsAny<List<DiscountItem>>(), 
                    It.IsAny<List<Domain.Model.Grocery>>()), 
                Times.Never);
    }

    [Test]
    public void GroceriesService_BuyGroceries_ShouldReturnValidShoppingBill()
    {
        // Arrange
        var userGroceries = new string[] {"apples", "soup"};
        
        var dataGroceries = this.GetDataGroceries(userGroceries);
        var domainGroceries = this.GetDomainGroceries(userGroceries);

        this.mockGroceryRepository
        .Setup(s => s.GetGroceriesPrices(userGroceries))
        .Returns(dataGroceries);

        this.mockGroceriesMapper
            .Setup(s => s.Map(userGroceries, dataGroceries))
            .Returns(domainGroceries);

        // Act
        var result = this.service.BuyGroceries(userGroceries);

        // Assert
        this.mockGroceryRepository
            .Verify(s => s.GetGroceriesPrices(userGroceries), Times.Once);

        this.mockGroceriesMapper
            .Verify(s => s.Map(It.IsAny<string[]>(), It.IsAny<List<Data.Model.Grocery>>()), Times.Once);

        this.mockDiscountStrategyManager
            .Verify(s => s.GetDiscountItems(It.IsAny<List<Domain.Model.Grocery>>()), Times.Once);
        

        Assert.That(domainGroceries.Sum(s => s.Price), Is.EqualTo(result.SubTotalPrice));
        Assert.That(result.TotalPrice, Is.EqualTo(result.SubTotalPrice));
        Assert.IsNull(result.DiscountItems);
    }

    [Test]
    public void GroceriesService_BuyGroceries_WithDiscount_ShouldReturnDiscounts()
    {
        // Arrange
        var userGroceries = new string[] {"apples", "soup"};
        var expectedTotalPrice = fixture.Create<double>();

        var dataGroceries = this.GetDataGroceries(userGroceries);
        this.mockGroceryRepository
            .Setup(s => s.GetGroceriesPrices(userGroceries))
            .Returns(dataGroceries);

        var domainGroceries = this.GetDomainGroceries(userGroceries);
        this.mockGroceriesMapper
            .Setup(s => s.Map(userGroceries, dataGroceries))
            .Returns(domainGroceries);

        var discountItems = this.GetDiscountItems();
        this.mockDiscountStrategyManager
            .Setup(s => s.GetDiscountItems(It.IsAny<List<Domain.Model.Grocery>>()))
            .Returns(discountItems);

        this.mockDiscountService
            .Setup(s => s.GetTotalPrice(discountItems, domainGroceries))
            .Returns(expectedTotalPrice);

        // Act
        var result = this.service.BuyGroceries(userGroceries);

        // Assert
        this.mockGroceryRepository
            .Verify(s => s.GetGroceriesPrices(userGroceries), Times.Once);

        this.mockGroceriesMapper
            .Verify(s => s.Map(It.IsAny<string[]>(), It.IsAny<List<Data.Model.Grocery>>()), Times.Once);

        this.mockDiscountStrategyManager
            .Verify(s => s.GetDiscountItems(It.IsAny<List<Domain.Model.Grocery>>()), Times.Once);

        this.mockDiscountService
            .Verify(s => s.GetTotalPrice(discountItems, domainGroceries), Times.Once);
        
        Assert.That(domainGroceries.Sum(s => s.Price), Is.EqualTo(result.SubTotalPrice));
        Assert.That(expectedTotalPrice, Is.EqualTo(result.TotalPrice));
        Assert.IsNotNull(result.DiscountItems);
        Assert.That(discountItems.Count, Is.EqualTo(result.DiscountItems.Count));
    }

    private List<Data.Model.Grocery> GetDataGroceries(params string[] groceriesNames)
    {
        return groceriesNames
            .Select(groceryName => 
                fixture
                    .Build<Data.Model.Grocery>()
                    .With(p => p.Name, groceryName)
                    .Create())
            .ToList();
    }

    private List<Domain.Model.Grocery> GetDomainGroceries(params string[] groceriesNames)
    {
        return groceriesNames
            .Select(groceryName => 
                fixture
                    .Build<Domain.Model.Grocery>()
                    .With(p => p.Name, groceryName)
                    .Create())
            .ToList();
    }

    private List<DiscountItem> GetDiscountItems()
    {
        return fixture.CreateMany<DiscountItem>().ToList();
    }
}