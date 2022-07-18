namespace ShoppingBasket.Domain.Model;

public class DiscountItem
{
    public string Description { get; set; }

    public string GroceryToApplyDiscount { get; set; }

    public double DiscountApplied { get; set; }

    public double DiscountValuePercentage { get; set; }
}
