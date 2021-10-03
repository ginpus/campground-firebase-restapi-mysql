using Domain.Client.Models.ResponseModels;
using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IUserService
    {
        Task<UserResponseModel> SignUpAsync(UserRequestModel user);

        Task<UserResponseModel> GetUserAsync(string localId);

        Task<SignInUserResponse> SignInUserAsync(UserRequestModel user);
    }
}
