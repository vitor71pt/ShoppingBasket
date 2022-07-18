namespace ShoppingBasket.Domain.Service.Strategies;

using ShoppingBasket.Domain.Model;

public class SoupDiscountStrategy : IDiscountStrategy
{
    public const string breadGroceryName = "bread";
    public const string soupGroceryName = "soup";

    public bool IsStrategyApplied(List<Grocery> groceries)
    {
        var soupItemsCount = 0;
        var hasBread = false;

        foreach (var grocery in groceries)
        {
            if (grocery.Name == soupGroceryName)
            {
                soupItemsCount = soupItemsCount + 1;
            }

            if (grocery.Name == breadGroceryName)
            {
                hasBread = true;
            }

            if (soupItemsCount == 2 && hasBread)
            {
                return true;
            }
        }

        return false;
    }

    public DiscountItem GetDiscount()
    {
        return new DiscountItem
        {
            Description = "Bread 50% off:",
            DiscountValuePercentage = 50,
            GroceryToApplyDiscount = breadGroceryName
        };
    }
}
