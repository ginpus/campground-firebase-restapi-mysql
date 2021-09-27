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
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<UserResponseModel> GetUserAsync(string localId)
        {
            var user = await _usersRepository.GetUserAsync(localId);

            return user.AsDto();
        }

        public async Task<UserResponseModel> SaveUserAsync(UserRequestModel user)
        {
            var userToSave = new UserResponseModel
            {
                UserId = Guid.NewGuid(),
                Email = user.Email,
                LocalId = user.LocalId,
                DateCreated = DateTime.Now
            };

            await _usersRepository.CreateUserAysnc(userToSave.AsDto());

            return userToSave;
        }
    }
}
