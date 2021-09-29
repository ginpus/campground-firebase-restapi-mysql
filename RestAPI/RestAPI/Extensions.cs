using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using RestAPI.Models.ResponseModels;

namespace RestAPI
{
    public static class Extensions
    {
        public static CampgroundResponse AsDto(this CampgroundRequestModel campground)
        {
            return new CampgroundResponse
            {
                CampgroundId = campground.CampgroundId,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated
            };
        }

        public static CampgroundResponse AsDto(this CampgroundResponseModel campground)
        {
            return new CampgroundResponse
            {
                CampgroundId = campground.CampgroundId,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated
            };
        }

        public static ImageResponse AsDto(this ImageResponseModel image)
        {
            return new ImageResponse
            {
                ImageId = image.ImageId,
                CampgroundId = image.CampgroundId,
                Url = image.Url
            };
        }
    }
}
