namespace ShoppingBasket.Domain.Service.Services;

using ShoppingBasket.Domain.Model;
using ShoppingBasket.Data.Repository;

public interface IDiscountService
{
    double GetTotalPrice(
        List<DiscountItem> discounts,
        List<Grocery> groceries
    );
}
