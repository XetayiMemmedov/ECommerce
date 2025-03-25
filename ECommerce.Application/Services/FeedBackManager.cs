using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Extensions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.EfCore.Context;
using ECommerce.Infrastructure.EfCore;
using ECommerce.UI;
using ECommerce.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace ECommerce.Application.Services
{
    public class FeedBackManager:IFeedBackService
    {
        
        private readonly IFeedBackRepository _repository;
        public FeedBackManager(IFeedBackRepository repository)
        {
            _repository = repository;

        }
        

        public void Add(FeedBackCreateDto createDto)
        {
            var feedback = createDto.ToFeedBack();

            _repository.Add(feedback);
        }

        public FeedBackDto Get(Expression<Func<FeedBack, bool>> predicate)
        {
            var feedBack = _repository.Get(predicate);

            var feedBackDto = feedBack.ToFeedBackDto();

            return feedBackDto;
        }

        public List<FeedBackDto> GetAll(Expression<Func<FeedBack, bool>>? predicate = null, bool asNoTracking = false)
        {
            var feedBacks = _repository.GetAll(predicate, asNoTracking);

            var feedBackDtoList = new List<FeedBackDto>();

            foreach (var item in feedBacks)
            {
                feedBackDtoList.Add(new FeedBackDto
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    UserFeedBacks = item.UserFeedBacks,
                });
            }

            return feedBackDtoList;
        }


        public FeedBackDto GetById(int id)
        {
            throw new NotImplementedException();

        }

        public void Remove(int id)
        {
            var existEntity = _repository.GetById(id);

            if (existEntity == null) throw new Exception("Not found");

            _repository.Remove(existEntity);
        }

        

        //public override void Add(FeedBackCreateDto createDto)
        //{
        //    var orders = orderService.GetAll(o => o.UserId == userId && o.OrderStatus == StatusType.Delivered);

        //    foreach (var order in orders)
        //    {
        //        var orderItem = order.Items.FirstOrDefault(i => i.Id == orderitemId);

        //        if (orderItem != null)
        //        {
        //            if (orderItem.Feedbacks == null)
        //                orderItem.Feedbacks = new List<UserFeedBack>();

        //            orderItem.Feedbacks.Add(new UserFeedBack
        //            {
        //                OrderItemId = orderItem.Id,
        //                FeedbackMark = feedback,
        //                CreatedAt = DateTime.Now
        //            });

        //            var feedBack = new FeedBack
        //            {
        //                UserId = userId,
        //                Feedbacks = orderItem.Feedbacks,
        //            };

        //            var feedbackcreatedto = MapExtensions.ToFeedBackCreateDto(feedBack);
        //            Add(feedbackcreatedto);
        //        }
        //    }
        //}

    }
}
