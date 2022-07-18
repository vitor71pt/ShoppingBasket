namespace ShoppingBasket.Domain.Service.Strategies;

using ShoppingBasket.Domain.Model;

public interface IDiscountStrategy
{
    bool IsStrategyApplied(List<Grocery> groceries);

    DiscountItem GetDiscount();
}
