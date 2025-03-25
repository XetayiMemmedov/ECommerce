namespace ECommerce.Domain.Entities
{
    public class UserFeedBack : Entity
    {
        public int FeedBackId { get; set; } 
        public FeedBack? FeedBack { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }



    }
}
