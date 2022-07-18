namespace ShoppingBasket.Domain.Service.Tests;

using ShoppingBasket.Domain.Service;
using ShoppingBasket.Domain.Model;
using ShoppingBasket.Domain.Service.Strategies;

public class DiscountStrategyManagerTests
{
    private DiscountStrategyManager strategyManager;

    [SetUp]
    public void Setup()
    {
        this.strategyManager = new DiscountStrategyManager(new List<IDiscountStrategy>
        {
            new AppleDiscountStrategy(), 
            new SoupDiscountStrategy()
        });
    }

    [Test]
    [TestCase("soup,bread,milk", 0)]
    [TestCase("apples", 1)]
    [TestCase("soup,bread,soup", 1)]
    [TestCase("soup,bread,milk,soup,apples", 2)]
    public void DiscountStrategyManager_GetDiscountItems_ShouldReturnCorrectDiscount(string groceriesName, int expectedDiscountCount)
    {
        // Arrange
        var groceries = groceriesName.Split(',')
        .Select(v => new Grocery
        {
            Name = v
        })
        .ToList();
        
        // Act
        var discountBills = this.strategyManager.GetDiscountItems(groceries);

        // Assert
        Assert.That(expectedDiscountCount, Is.EqualTo(discountBills.Count));
    }
}