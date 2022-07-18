namespace ShoppingBasket.Domain.Model;

public class ShoppingBill
{
    public double SubTotalPrice { get; set; }

    public double TotalPrice { get; set; }

    public List<DiscountItem> DiscountItems { get; set; }
}
