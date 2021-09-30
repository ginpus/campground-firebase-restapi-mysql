using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using Persistence.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CampgroundsService : ICampgroundsService
    {
        private readonly ICampgroundsRepository _campgroundsRepository;

        public CampgroundsService(ICampgroundsRepository campgroundsRepository)
        {
            _campgroundsRepository = campgroundsRepository;
        }

        public async Task<int> DeleteAllAsync(Guid userid)
        {
            var allCampgrounds = await _campgroundsRepository.GetAllUserItemsAsync(userid);

            var rowsAffected = await _campgroundsRepository.DeleteAllAsync(userid);

            if (rowsAffected > 0)
            {
                foreach (var campground in allCampgrounds)
                {
                    await _campgroundsRepository.DeleteAllRelatedImagesAsync(campground.CampgroundId);
                }
            }

            return rowsAffected;
        }

        public async Task<int> DeleteAsync(Guid campgroundid, Guid userid)
        {
            var rowsAffected = await _campgroundsRepository.DeleteAsync(campgroundid, userid);

            if (rowsAffected > 0)
            {
                await _campgroundsRepository.DeleteAllRelatedImagesAsync(campgroundid);
            }

            return rowsAffected;
        }


        public async Task<IEnumerable<CampgroundResponseModel>> GetAllUserItemsAsync(Guid userid)
        {
            var allItems = await _campgroundsRepository.GetAllUserItemsAsync(userid);

            var allCampgrounds = new List<CampgroundResponseModel>();

            foreach (var item in allItems)
            {
                var allImages = (await _campgroundsRepository.GetImagesByCampgroundIdAsync(item.CampgroundId))
                    .Select(image => image.AsDto());

                var campground = new CampgroundResponseModel
                {
                    CampgroundId = item.CampgroundId,
                    UserId = item.UserId,
                    Name = item.Name,
                    Price = item.Price,
                    Images = allImages,
                    Description = item.Description,
                    DateCreated = item.DateCreated
                };

                allCampgrounds.Add(campground);
            }

            return allCampgrounds;
        }

        public async Task<IEnumerable<CampgroundResponseModel>> GetAllItemsAsync()
        {
            var allItems = await _campgroundsRepository.GetAllAsync();

            var allCampgrounds = new List<CampgroundResponseModel> { };

            foreach (var item in allItems)
            {
                var allImages = (await _campgroundsRepository.GetImagesByCampgroundIdAsync(item.CampgroundId))
                    .Select(image => image.AsDto());

                var campground = new CampgroundResponseModel
                {
                    CampgroundId = item.CampgroundId,
                    UserId = item.UserId,
                    Name = item.Name,
                    Price = item.Price,
                    Images = allImages,
                    Description = item.Description,
                    DateCreated = item.DateCreated
                };

                allCampgrounds.Add(campground);
            }

            return allCampgrounds;
        }

        public async Task<CampgroundResponseModel> GetItemByIdAsync(Guid campgroundid, Guid userid)
        {
            var item = await _campgroundsRepository.GetItemByIdAsync(campgroundid, userid);

            return item.AsDto();
        }

        public async Task<int> CreateOrEditAsync(CampgroundRequestModel campground)
        {
            var newCampground = new CampgroundRequestModel
            {
                CampgroundId = campground.CampgroundId,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated
            };

            var rowsAffected = await _campgroundsRepository.SaveOrUpdateAsync(newCampground.AsDto());

            return rowsAffected;
        }

        public async Task<IEnumerable<ImageResponseModel>> GetImagesByCampgroundIdAsync(Guid campgroundid)
        {
            var allImages = (await _campgroundsRepository.GetImagesByCampgroundIdAsync(campgroundid))
                .Select(image => image.AsDto());

            return allImages;
        }

        public async Task<ImageResponseModel> CreateOrEditImageAsync(ImageRequestModel image, Guid userId)
        {
            var allUserCampgrounds = await _campgroundsRepository.GetAllUserItemsAsync(userId);

            var campgroundBelongsToUser = allUserCampgrounds.FirstOrDefault(userCampground => userCampground.CampgroundId == image.CampgroundId);

            if (campgroundBelongsToUser is not null)
            {
                var newImage = new ImageRequestModel
                {
                    ImageId = image.ImageId,
                    CampgroundId = image.CampgroundId,
                    Url = image.Url
                };

                await _campgroundsRepository.SaveOrUpdateImageAsync(newImage.AsDto());

                return new ImageResponseModel
                {
                    ImageId = newImage.ImageId,
                    CampgroundId = newImage.CampgroundId,
                    Url = newImage.Url
                };
            }
            else
            {
                throw new Exception("There is no such campground for your user");
            }

        }

        public async Task<int> DeleteImageAsync(Guid imageid, Guid userid)
        {
            var allUserCampgrounds = (await _campgroundsRepository.GetAllUserItemsAsync(userid))
                .ToList();

            var userIdFromDb = await _campgroundsRepository.GetUserFromImageId(imageid);

            if (userIdFromDb == userid)
            {
                var rowsAffected = await _campgroundsRepository.DeleteImageAsync(imageid);

                return rowsAffected;
            }

            return 0;
        }
    }
}
