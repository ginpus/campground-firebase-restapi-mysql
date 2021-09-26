﻿using Persistence.Client;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private const string UsersTable = "users";
        private readonly ISqlClient _sqlClient;

        public UsersRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public async Task<int> CreateUserAysnc(UserWriteModel user)
        {
            var sqlInsert = @$"INSERT INTO {UsersTable} (userid, email, localid, datecreated) VALUES(@userid, @email, @localid, @datecreated)";

            var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
            {
                userid = user.UserId,
                email = user.Email,
                localid = user.LocalId,
                datecreated = user.DateCreated
            });

            return await rowsAffected;
        }

        public async Task<IEnumerable<UserReadModel>> GetAllUsersAsync()
        {
            var sqlSelect = $"SELECT userid, email, localid, datecreated FROM {UsersTable}";

            var users = await _sqlClient.QueryAsync<UserReadModel>(sqlSelect);

            return users;
        }

        public async Task<UserReadModel> GetUserAsync(string email)
        {
            var sqlSelect = $"SELECT userid, email, localid, datecreated FROM {UsersTable} where email = @email";

            var user = await _sqlClient.QuerySingleOrDefaultAsync<UserReadModel>(sqlSelect, new
            {
                email = email
            });

            return user;
        }
    }
}
