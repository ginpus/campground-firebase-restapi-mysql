using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface ICampgroundsRepository
    {
        Task<IEnumerable<CampgroundReadModel>> GetAllAsync();

        Task<CampgroundReadModel> GetItemByIdAsync(Guid campgroundid, Guid userid);

        Task<IEnumerable<CampgroundReadModel>> GetAllUserItemsAsync(Guid userid);

        Task<int> DeleteAsync(Guid campgroundid, Guid userid);

        Task<int> DeleteAllAsync(Guid userid);

        Task<int> SaveOrUpdateAsync(CampgroundWriteModel campground);

        Task<IEnumerable<ImageReadModel>> GetImagesByCampgroundIdAsync(Guid campgroundid);

        Task<int> DeleteAllRelatedImagesAsync(Guid campgroundid);

        Task<int> SaveOrUpdateImageAsync(ImageWriteModel image);

        Task<int> DeleteImageAsync(Guid imageid);

        Task<Guid> GetUserFromImageId(Guid imageid);
    }
}
