namespace ShoppingBasket.Data.Repository;

using ShoppingBasket.Data.Model;

public interface IGroceryRepository
{
    List<Grocery> GetGroceriesPrices(params string[] groceriesName); 
}