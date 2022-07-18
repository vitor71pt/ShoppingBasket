namespace ShoppingBasket.Domain.Service.Strategies;

using ShoppingBasket.Domain.Model;

public class AppleDiscountStrategy : IDiscountStrategy
{
    public const string appleGroceryName = "apples";

    public bool IsStrategyApplied(List<Grocery> groceries)
    {
        return groceries.FirstOrDefault(grocery => grocery.Name == appleGroceryName) != null;
    }

    public DiscountItem GetDiscount()
    {
        return new DiscountItem
        {
            Description = "Apples 10% off:",
            DiscountValuePercentage = 10,
            GroceryToApplyDiscount = appleGroceryName
        };
    }
}
