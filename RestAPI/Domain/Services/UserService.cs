﻿using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using Domain.Client;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Client.Models.ResponseModels;

namespace Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IAuthClient _authClient;

        public UserService(IUsersRepository usersRepository, IAuthClient authClient)
        {
            _usersRepository = usersRepository;
            _authClient = authClient;
        }

        public async Task<UserResponseModel> GetUserAsync(string localId)
        {
            var user = await _usersRepository.GetUserAsync(localId);

            return user.AsDto();
        }

        public async Task<UserResponseModel> SignUpAsync(UserRequestModel user)
        {

            var newUser = await _authClient.SignUpUserAsync(user.Email, user.Password);

            var userToSave = new UserResponseModel
            {
                UserId = Guid.NewGuid(),
                Email = newUser.Email,
                LocalId = newUser.LocalId,
                DateCreated = DateTime.Now
            };

            await _usersRepository.CreateUserAysnc(userToSave.AsDto());

            return userToSave;
        }

        public async Task<SignInUserResponse> SignInUserAsync(UserRequestModel user)
        {
            var returnedUser = await _authClient.SignInUserAsync(user.Email, user.Password);

            return returnedUser;
        }
    }
}
