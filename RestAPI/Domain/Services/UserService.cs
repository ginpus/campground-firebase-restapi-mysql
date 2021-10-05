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
using Contracts.ResponseModels;
using Contracts.RequestModels;

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

        public async Task<UserResponseModel> SignUpAsync(SignUpRequest user)
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

        public async Task<SignInResponse> SignInUserAsync(SignInRequest user)
        {
            var returnedUser = await _authClient.SignInUserAsync(user.Email, user.Password);

            var userFromDb = await _usersRepository.GetUserAsync(returnedUser.LocalId);

            return new SignInResponse
            {
                Email = userFromDb.Email,
                IdToken = returnedUser.IdToken
            };
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequestModel request)
        {
            var response = await _authClient.ChangeUserPasswordAsync(request);

            return response.AsDto();
        }
    }
}
