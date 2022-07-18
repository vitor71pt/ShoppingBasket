namespace ShoppingBasket.Domain.Service.Services;

using ShoppingBasket.Domain.Model;

public interface IGroceriesService
{
    ShoppingBill BuyGroceries(params string[] groceries);
}
