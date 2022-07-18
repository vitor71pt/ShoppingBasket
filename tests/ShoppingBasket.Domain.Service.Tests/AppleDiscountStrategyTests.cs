namespace ShoppingBasket.Domain.Service.Tests;

using ShoppingBasket.Domain.Model;
using ShoppingBasket.Domain.Service.Strategies;

public class AppleDiscountStrategyTests
{
    private AppleDiscountStrategy strategy;

    [SetUp]
    public void Setup()
    {
        this.strategy = new AppleDiscountStrategy();
    }

    [Test]
    [TestCase("apples", true)]
    [TestCase("random", false)]
    public void AppleDiscountStrategy_IsStrategyApplied_ShouldReturnTrue(
        string groceryName,
        bool expectedIsStrategyApplied)
    {
        // Arrange
        var groceries = new List<Grocery>
        {
            new Grocery
            {
                Name = groceryName
            }
        };

        // Act
        var isStrategyApplied = this.strategy.IsStrategyApplied(groceries);

        // Assert
        Assert.That(expectedIsStrategyApplied, Is.EqualTo(isStrategyApplied));
    }

    [Test]
    public void AppleDiscountStrategy_GetDiscount_ShouldReturnDiscountBill()
    {
        // Arrange, 
        var expectedDescription = "Apples 10% off:";
        var expectedDiscountValuePercentage = 10;
        var expectedGroceryToApplyDiscount = "apples";

        // Act
        var discountBill = this.strategy.GetDiscount();

        // Assert
        Assert.That(expectedDescription, Is.EqualTo(discountBill.Description));
        Assert.That(expectedDiscountValuePercentage, Is.EqualTo(discountBill.DiscountValuePercentage));
        Assert.That(expectedGroceryToApplyDiscount, Is.EqualTo(discountBill.GroceryToApplyDiscount));
    }
}