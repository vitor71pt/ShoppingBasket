namespace ShoppingBasket.Domain.Service.Tests;

using ShoppingBasket.Domain.Model;
using AutoFixture;
using ShoppingBasket.Domain.Service.Services;

public class DiscountServiceTests
{
    private static Fixture fixture = new Fixture();

    private const double discountPercentage = 10;

    private DiscountService discountService;

    [SetUp]
    public void Setup()
    {
        this.discountService = new DiscountService();
    }

    [Test]
    public void DiscountService_GetTotalPrice_ShouldReturnValidPrice()
    {
        // Arrange
        var groceriesNames = new string[] {
            "apples",
            "bread"
        };

        var discountItems = groceriesNames
        .Select(groceryName => new DiscountItem{
            GroceryToApplyDiscount = groceryName,
            DiscountValuePercentage = discountPercentage
        })
        .ToList();

        var groceries = this.GetGroceries(groceriesNames);
        var expectedSubTotalPrice = groceries.Sum(s =>s.Price);

        // Act
        var totalPrice = this.discountService.GetTotalPrice(discountItems, groceries);

        // Assert
        var totalDiscounts = discountItems.Sum(s =>s.DiscountApplied);

        Assert.IsTrue(totalPrice > 0);
        Assert.That(expectedSubTotalPrice, Is.Not.EqualTo(totalPrice));

        var groceriesPriceByName = groceries.ToDictionary(k => k.Name, v => v.Price);

        foreach(var discount in discountItems)
        {
            var groceryPrice = groceriesPriceByName[discount.GroceryToApplyDiscount];

            Assert.That(groceryPrice * (discountPercentage/100), Is.EqualTo(discount.DiscountApplied));
        }

        Assert.That(expectedSubTotalPrice - totalDiscounts, Is.EqualTo(totalPrice));
    }

    
    private List<Domain.Model.Grocery> GetGroceries(params string[] groceriesNames)
    {
        return groceriesNames
            .Select(groceryName => 
                fixture
                    .Build<Domain.Model.Grocery>()
                    .With(p => p.Name, groceryName)
                    .Create())
            .ToList();
    }
}