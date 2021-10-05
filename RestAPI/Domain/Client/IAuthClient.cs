﻿using Domain.Client.Models.RequestModels;
using Domain.Client.Models.ResponseModels;
using Domain.Models.RequestModels;
using System.Threading.Tasks;

namespace Domain.Client
{
    public interface IAuthClient
    {
        Task<CreateUserResponse> SignUpUserAsync(string email, string password);

        Task<ClientSignInUserResponse> SignInUserAsync(string email, string password);

        Task<ClientChangePasswordOrEmailResponse> ChangeUserPasswordAsync(ChangePasswordRequestModel request);

        Task<ClientChangePasswordOrEmailResponse> ChangeUserEmailAsync(ChangeEmailRequestModel request);
    }
}
