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
        Task<CampgroundResponseModel> GetCampgroundByIdAsync(Guid campgroundid, Guid userid);

        Task<CampgroundResponseModel> GetCampgroundAsync(Guid campgroundid);

        Task<IEnumerable<ImageResponseModel>> GetImagesByCampgroundIdAsync(Guid campgroundid);

        Task<IEnumerable<CampgroundResponseModel>> GetAllUserItemsAsync(Guid userid);

        Task<IEnumerable<CampgroundResponseModel>> GetAllItemsAsync();

        Task<int> CreateOrEditAsync(CampgroundRequestModel campground);

        Task<int> DeleteAsync(Guid campgroundid, Guid userid);

        Task<int> DeleteAllAsync(Guid userid);

        Task<ImageResponseModel> CreateOrEditImageAsync(ImageRequestModel image, Guid userId);

        Task<int> DeleteImageAsync(Guid imageid, Guid userid);
    }
}
