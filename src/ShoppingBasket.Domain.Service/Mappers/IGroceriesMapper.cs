namespace ShoppingBasket.Domain.Service.Mappers;

public interface IGroceriesMapper
{
    List<Model.Grocery> Map(
       string[] userGroceries,
       List<Data.Model.Grocery> groceries
    );
}
