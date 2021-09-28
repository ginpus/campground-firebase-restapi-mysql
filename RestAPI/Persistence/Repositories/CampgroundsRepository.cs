using Persistence.Client;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CampgroundsRepository : ICampgroundsRepository
    {
        private const string CampgroundsTable = "campground";
        private readonly ISqlClient _sqlClient;

        public CampgroundsRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public async Task<int> DeleteAllAsync(Guid userid)
        {
            var sqlDeleteAll = $"DELETE FROM {CampgroundsTable} WHERE userid = @userid";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlDeleteAll, new
            {
                userid = userid
            });
            return rowsAffected;
        }

        public async Task<int> DeleteAsync(Guid campgroundid, Guid userid)
        {
            var sqlDelete = $"DELETE FROM {CampgroundsTable} WHERE campgroundid = @campgroundid AND userid = @userid";

            Console.WriteLine($"CampgroundId: {campgroundid}");
            Console.WriteLine($"UserID: {userid}");
            Console.WriteLine($"SQL: {sqlDelete}");

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlDelete, new
            {
                campgroundid = campgroundid,
                userid = userid
            });
            return rowsAffected;
        }

        public async Task<IEnumerable<CampgroundReadModel>> GetAllAsync()
        {
            var sqlSelect = $"SELECT campgroundid, userid, name, price, description, datecreated FROM {CampgroundsTable} ORDER BY datecreated desc";

            return await _sqlClient.QueryAsync<CampgroundReadModel>(sqlSelect);
        }

        public async Task<IEnumerable<CampgroundReadModel>> GetAllUserItemsAsync(Guid userid)
        {
            var sqlSelect = $"SELECT campgroundid, userid, name, price, description, datecreated FROM {CampgroundsTable} WHERE userid = @userid ORDER BY datecreated desc";

            return await _sqlClient.QueryAsync<CampgroundReadModel>(sqlSelect, new
            {
                userid = userid
            });
        }

        public async Task<CampgroundReadModel> GetItemByIdAsync(Guid campgroundid, Guid userid)
        {
            var sqlSelect = $"SELECT campgroundid, userid, name, price, description, datecreated FROM {CampgroundsTable} WHERE campgroundid = @campgroundid AND userid = @userid";

            return await _sqlClient.QueryFirstOrDefaultAsync<CampgroundReadModel>(sqlSelect, new
            {
                campgroundid = campgroundid,
                userid = userid
            });
        }

        public async Task<int> SaveOrUpdate(CampgroundWriteModel campground)
        {
            var sql = @$"INSERT INTO {CampgroundsTable} (campgroundid, userid, name, price, description, datecreated) VALUES(@campgroundid, @userid, @name, @price, @description, @datecreated) ON DUPLICATE KEY UPDATE name = @name, description = @description, price = @price";

            var rowsAffected = _sqlClient.ExecuteAsync(sql, new
            {
                campgroundid = campground.CampgroundId,
                userid = campground.UserId,
                name = campground.Name,
                price = campground.Price,
                description = campground.Description,
                datecreated = campground.DateCreated
            });

            return await rowsAffected;
        }
    }
}
