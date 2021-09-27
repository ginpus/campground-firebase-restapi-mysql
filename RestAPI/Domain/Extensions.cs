using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class Extensions
    {
        public static UserWriteModel AsDto(this UserResponseModel user)
        {
            return new UserWriteModel
            {
                UserId = user.UserId,
                Email = user.Email,
                LocalId = user.LocalId,
                DateCreated = user.DateCreated
            };
        }

        public static UserResponseModel AsDto(this UserReadModel user)
        {
            return new UserResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                LocalId = user.LocalId,
                DateCreated = user.DateCreated
            };
        }

        public static CampgroundWriteModel AsDto(this CampgroundRequestModel campground)
        {
            return new CampgroundWriteModel
            {
                CampgroundId = campground.CampgroundId,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated
            };
        }
    }
}
