using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using Persistence.Repositories;
using System;
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
            var rowsAffected = await _campgroundsRepository.DeleteAllAsync(userid);

            return rowsAffected;
        }

        public async Task<int> DeleteAsync(Guid campgroundid, Guid userid)
        {
            var rowsAffected = await _campgroundsRepository.DeleteAsync(campgroundid, userid);

            return rowsAffected;
        }

        public Task<int> EditAsync(Guid campgroundid, Guid userid, UpdateCampgroundRequestModel campground)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CampgroundResponseModel>> GetAllUserItemsAsync(Guid userid)
        {
            var allItems = (await _campgroundsRepository.GetAllUserItemsAsync(userid))
                .Select(item => item.AsDto());

            return allItems;
        }

        public async Task<IEnumerable<CampgroundResponseModel>> GetAllItemsAsync()
        {
            var allItems = (await _campgroundsRepository.GetAllAsync())
                .Select(item => item.AsDto());

            return allItems;
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

            var rowsAffected = await _campgroundsRepository.SaveOrUpdate(newCampground.AsDto());

            return rowsAffected;
        }
    }
}
