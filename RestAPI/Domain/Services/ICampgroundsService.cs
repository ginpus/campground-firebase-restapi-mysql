using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ICampgroundsService
    {
        Task<CampgroundResponseModel> GetItemByIdAsync(Guid campgroundid, Guid userid);

        Task<IEnumerable<ImageResponseModel>> GetImagesByCampgroundIdAsync(Guid campgroundid);

        Task<IEnumerable<CampgroundResponseModel>> GetAllUserItemsAsync(Guid userid);

        Task<IEnumerable<CampgroundResponseModel>> GetAllItemsAsync();

        Task<int> CreateOrEditAsync(CampgroundRequestModel campground);

        Task<int> DeleteAsync(Guid campgroundid, Guid userid);

        Task<int> DeleteAllAsync(Guid userid);
    }
}
