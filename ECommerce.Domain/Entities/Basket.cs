namespace ECommerce.UI;

public class Basket
{
    public int UserId { get; set; }
    public List<BasketItem> Items { get; set; }
    public string PromoCode { get; set; }

    public Basket(int userId, string? promoCode = null)
    {
        UserId = userId;
        Items = new List<BasketItem>();
        PromoCode = promoCode;
    }
}
