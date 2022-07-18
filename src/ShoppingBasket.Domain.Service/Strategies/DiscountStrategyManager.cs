namespace ShoppingBasket.Domain.Service.Strategies;

using ShoppingBasket.Domain.Model;

public class DiscountStrategyManager : IDiscountStrategyManager
{
    private List<IDiscountStrategy> discountStrategies;

    public DiscountStrategyManager(List<IDiscountStrategy> discountStrategies)
    {
        this.discountStrategies = discountStrategies;
    }

    public List<DiscountItem> GetDiscountItems(List<Grocery> groceries)
    {
        var strategies = this.discountStrategies
                .Where(v => v.IsStrategyApplied(groceries))
                .ToList();

        return strategies.Select(strategy => strategy.GetDiscount()).ToList();
    }
}
