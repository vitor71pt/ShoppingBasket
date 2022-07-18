namespace ShoppingBasket.Domain.Service.Services;

using ShoppingBasket.Domain.Model;

public class DiscountService : IDiscountService
{
    public double GetTotalPrice(
        List<DiscountItem> discounts,
        List<Grocery> groceries)
    {
        foreach (var discount in discounts)
        {
            foreach (var grocery in groceries)
            {
                if (grocery.Name == discount.GroceryToApplyDiscount)
                {
                    var discountToApply = grocery.Price * (discount.DiscountValuePercentage / 100);
                    discount.DiscountApplied = discountToApply;
                }
            }
        }

        return groceries.Sum(v => v.Price) - discounts.Sum(d => d.DiscountApplied);
    }
}
