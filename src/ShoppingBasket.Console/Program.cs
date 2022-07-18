// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using ShoppingBasket.Domain.Service.Mappers;
using ShoppingBasket.Data.Repository;
using ShoppingBasket.Domain.Service.Strategies;
using ShoppingBasket.Domain.Service.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;


Console.WriteLine("Shopping Cost");
StringBuilder errorMessage = new StringBuilder("Whoops! something went wrong");

IGroceriesService groceriesService = new GroceriesService(
    new GroceryRepository(),
    new DiscountStrategyManager(
        new List<IDiscountStrategy>
        {
            new AppleDiscountStrategy(),
            new SoupDiscountStrategy()
        }),
    new DiscountService(),
    new GroceriesMapper());

if(args.Length == 0)
{   
    errorMessage.Append(", You need to Pass At least one grocery.");
    Console.WriteLine(errorMessage);

    return;
}

var shoppingBill = groceriesService?.BuyGroceries(args);

if(shoppingBill == null)
{
    errorMessage.Append("can't find item(s) with the name");
    errorMessage.Append(" ");
    errorMessage.Append(string.Join(",", args));

    Console.WriteLine(errorMessage);

    return;
}

Console.WriteLine("Subtotal: " + "€" + PrintDouble(shoppingBill.SubTotalPrice));

if(shoppingBill.DiscountItems == null || !shoppingBill.DiscountItems.Any())
{
    Console.WriteLine("(No offers available)");
}else{  
    foreach (var item in shoppingBill.DiscountItems)
    {
        Console.WriteLine(item.Description+" -€"+PrintDouble(item.DiscountApplied));
    }
}

Console.WriteLine("Total price: " + "€"+PrintDouble(shoppingBill.TotalPrice));

double PrintDouble(double val)
{
    return Math.Round(val, 2);
}