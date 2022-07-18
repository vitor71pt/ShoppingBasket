namespace ShoppingBasket.Data.Repository;

using ShoppingBasket.Data.Model;

public class GroceryRepository : IGroceryRepository
{
    public List<Grocery> GetGroceriesPrices(params string[] groceriesName)
    {
        var groceries = this.GetGroceries();

        return groceries.Select(g => g).Where(g => groceriesName.Contains(g.Name)).ToList();
    }

    private List<Grocery> GetGroceries()
    {
        var groceries = new List<Grocery>();

        foreach (string line in System.IO.File.ReadLines("./groceries.csv"))
        {  
            var splitLine = line.Split(";");

            if(splitLine.Length < 2)
            {
                continue;
            }

            groceries.Add(new Grocery 
            {
                Name = splitLine[0],
                Price = double.Parse(splitLine[1])
            });
        }  

        return groceries;
    }
}