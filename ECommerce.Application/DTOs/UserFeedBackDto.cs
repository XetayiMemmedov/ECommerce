using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public class UserFeedBackDto
    {
        public int Id { get; set; }
        public FeedBack? FeedBack { get; set; }
        public Product? Product { get; set; }

    }

    public class UserFeedBackCreateDto
    {
        public int FeedBackId { get; set; }
        public int ProductId { get; set; }

    }
}
