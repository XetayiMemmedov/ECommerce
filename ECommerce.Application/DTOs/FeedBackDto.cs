using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs;

public class FeedBackDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public double FeedBackMark { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<UserFeedBack>? UserFeedBacks { get; set; } 


}

public class FeedBackCreateDto
{
    public List<UserFeedBack> UserFeedBacks { get; set; }= new List<UserFeedBack>();
    public int UserId { get; set; }
    public double FeedBackMark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}

public class FeedBackUpdateDto
{
    public List<UserFeedBack> UserFeedBacks { get; set; } = new List<UserFeedBack>();
    public int UserId { get; set; }
    public double FeedBackMark { get; set; }
    public DateTime CreatedAt { get; set; }

}