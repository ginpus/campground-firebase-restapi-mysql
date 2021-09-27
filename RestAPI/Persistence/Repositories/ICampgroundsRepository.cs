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

        Task<int> SaveAsync(CampgroundWriteModel campground);

        Task<int> EditAsync(Guid campgroundid, UpdateCampgroundWriteModel campground, Guid userid);

        Task<int> DeleteAsync(Guid campgroundid, Guid userid);

        Task<int> DeleteAllAsync(Guid userid);

        Task<int> SaveOrUpdate(CampgroundWriteModel campground);
    }
}
