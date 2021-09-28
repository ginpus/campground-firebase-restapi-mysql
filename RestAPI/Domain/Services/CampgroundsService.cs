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

        public Task<int> DeleteAllAsync(Guid userid)
        {
            var rowsAffected = _campgroundsRepository.DeleteAllAsync(userid);

            return rowsAffected;
        }

        public Task<int> DeleteAsync(Guid campgroundid, Guid userid)
        {
            var rowsAffected = _campgroundsRepository.DeleteAsync(campgroundid, userid);

            return rowsAffected;
        }

        public Task<int> EditAsync(Guid campgroundid, UpdateCampgroundRequestModel campground, Guid userid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CampgroundResponseModel>> GetAllUserItemsAsync(Guid userid)
        {
            var allItems = (await _campgroundsRepository.GetAllUserItemsAsync(userid))
                .Select(item => item.AsDto());

            return allItems;
        }

        public Task<CampgroundResponseModel> GetItemByIdAsync(Guid campgroundid, Guid userid)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(CampgroundRequestModel campground)
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

            var rowsAffected = _campgroundsRepository.SaveOrUpdate(newCampground.AsDto());

            return rowsAffected;
        }
    }
}
