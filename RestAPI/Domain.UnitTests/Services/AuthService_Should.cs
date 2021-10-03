using Contracts.RequestModels;
using Domain.Services;
using Moq;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class AuthService_Should
    {
        //Given_When_Then
        [Fact]
        public async Task SignInAsync_WithSignInRequest_ReturnSignInResponse()
        {
            /*            //Arange - preparation of data, creation of models/classes
                        var authClientMock = new Mock<IAuthClient>();
                        var userRepositoryMock = new Mock<IUsersRepository>();

                        var signInrequest = new SignInRequest
                        {
                            Email = Guid.NewGuid().ToString(),
                            Password = Guid.NewGuid().ToString()
                        };

                        //Setup defines what is going to happen when the method will be called
                        authClientMock
                            .Setup(authClient => authClient
                            .SignInAsync()

                        //sut - system under test
                        var sut = new UserService(authClientMock.Object, userRepositoryMock.Object);

                        //Act - we call the method we want to test

                        var result = await sut.SignInAsync();

                        //Assert - did the required methods were called, did the intended data were returned
            */
        }
    }
}
