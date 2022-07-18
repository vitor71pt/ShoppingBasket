namespace ShoppingBasket.Domain.Service.Mappers;

public class GroceriesMapper : IGroceriesMapper
{
    public List<Model.Grocery> Map(
       string[] userGroceries,
       List<Data.Model.Grocery> groceries)
    {
        var domainModelGroceries = new List<Model.Grocery>();

        foreach (var userGrocery in userGroceries)
        {
            var currentGrocery = groceries.FirstOrDefault(grocery => grocery.Name == userGrocery);

            if (currentGrocery == null)
            {
                continue;
            }

            domainModelGroceries.Add(new Model.Grocery
            {
                Name = currentGrocery.Name,
                Price = currentGrocery.Price
            });
        }

        return domainModelGroceries;
    }
}
