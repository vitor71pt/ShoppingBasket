namespace ShoppingBasket.Domain.Service.Strategies;

using ShoppingBasket.Domain.Model;

public interface IDiscountStrategyManager
{
    List<DiscountItem> GetDiscountItems(List<Grocery> groceries);
}
