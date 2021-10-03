using RestAPI.Client.Models.ResponseModels;
using System.Threading.Tasks;

namespace RestAPI.Client
{
    public interface IAuthClient
    {
        Task<CreateUserResponse> SignUpUserAsync(string email, string password);

        Task<SignInUserResponse> SignInUserAsync(string email, string password);
    }
}
