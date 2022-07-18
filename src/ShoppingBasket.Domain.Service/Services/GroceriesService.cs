namespace ShoppingBasket.Domain.Service.Services;

using ShoppingBasket.Domain.Model;
using ShoppingBasket.Data.Repository;
using ShoppingBasket.Domain.Service.Strategies;
using ShoppingBasket.Domain.Service.Mappers;

public class GroceriesService : IGroceriesService
{
    private IGroceryRepository groceriesRepository;
    private IDiscountStrategyManager discountStrategyManager;
    private IDiscountService discountService;
    private IGroceriesMapper groceriesMapper;

    public GroceriesService(
        IGroceryRepository groceriesRepository,
        IDiscountStrategyManager discountStrategyManager,
        IDiscountService discountService,
        IGroceriesMapper groceriesMapper)
    {
        this.groceriesRepository = groceriesRepository;
        this.discountStrategyManager = discountStrategyManager;
        this.discountService = discountService;
        this.groceriesMapper = groceriesMapper;
    }

    public ShoppingBill BuyGroceries(params string[] userGroceries)
    {
        var groceries = this.groceriesRepository.GetGroceriesPrices(userGroceries);

        if (groceries == null || !groceries.Any())
        {
            return null;
        }

        var domainModelGroceries = this.groceriesMapper.Map(userGroceries, groceries);

        var discounts = this.discountStrategyManager.GetDiscountItems(domainModelGroceries);
        var subTotalPrice = this.GetSubTotalPrice(domainModelGroceries);
        var totalPrice = subTotalPrice;

        if (discounts != null && discounts.Any())
        {
            totalPrice = this.discountService.GetTotalPrice(discounts, domainModelGroceries);
        }

        var shoppingBill = new ShoppingBill
        {
            TotalPrice = totalPrice,
            SubTotalPrice = subTotalPrice,
            DiscountItems = discounts
        };

        return shoppingBill;
    }

    private double GetSubTotalPrice(
        List<Grocery> groceries)
    {
        if (groceries == null)
        {
            return 0d;
        }

        return groceries.Sum(grocery => grocery.Price);
    }
}
