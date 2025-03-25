namespace ECommerce.Domain.Entities;

public class FeedBack : Entity
{
    public int UserId { get; set; }
    public User? User { get; set; }
    public double FeedBackMark { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<UserFeedBack> UserFeedBacks { get; set; } = new List<UserFeedBack>();

    

}

