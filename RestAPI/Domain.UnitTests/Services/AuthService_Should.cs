using Contracts.RequestModels;
using Contracts.ResponseModels;
using Domain.Client;
using Domain.Client.Models.RequestModels;
using Domain.Client.Models.ResponseModels;
using Domain.Models.RequestModels;
using Domain.Services;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
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
            //Arange - preparation of data, creation of mock data model objects
            var authClientMock = new Mock<IAuthClient>();
            var userRepositoryMock = new Mock<IUsersRepository>();

            // expected input - data that would be provided to the tested method (in this case AuthService method SignIn)
            var signInRequest = new SignInRequest
            {
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };

            //expected output from authClient
            var signInResponse = new ClientSignInUserResponse
            {
                IdToken = Guid.NewGuid().ToString(),
                Email = signInRequest.Email,
                LocalId = Guid.NewGuid().ToString()
            };

            //expected output from userRepository
            var userReadModel = new UserReadModel
            {
                UserId = Guid.NewGuid(),
                LocalId = signInResponse.LocalId,
                Email = signInResponse.Email,
                DateCreated = DateTime.Now
            };

            //Setup defines what is going to happen when the method will be called
            authClientMock
                 .Setup(authClient => authClient
                 .SignInUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync(signInResponse);

            userRepositoryMock
                .Setup(userRepository => userRepository
                .GetUserAsync(signInResponse.LocalId))
                .ReturnsAsync(userReadModel);

            var expectedResult = new SignInResponse
            {
                Email = userReadModel.Email,
                IdToken = signInResponse.IdToken
            };

            //sut - system under test
            var sut = new UserService(userRepositoryMock.Object, authClientMock.Object);

            //Act - we call the method we want to test

            var result = await sut.SignInUserAsync(signInRequest);

            //Assert - did the required methods were called, did the intended data were returned

            Assert.Equal(expectedResult.Email, result.Email);
            Assert.Equal(expectedResult.IdToken, result.IdToken);

            authClientMock
                .Verify(authClient => authClient.SignInUserAsync(signInRequest.Email, signInRequest.Password), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository.GetUserAsync(signInResponse.LocalId), Times.Once);

        }

        //Given_When_Then
        [Fact]
        public async Task SignUpAsync_WithSignUpRequest_ReturnSignUpResponse()
        {
            //Arange - preparation of data, creation of mock data model objects
            var authClientMock = new Mock<IAuthClient>();
            var userRepositoryMock = new Mock<IUsersRepository>();

            // expected input - data that would be provided to the tested method (in this case AuthService method SignIn)
            var signUpRequest = new SignUpRequest
            {
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };

            //expected output from authClient
            var signUpResponse = new CreateUserResponse
            {
                IdToken = Guid.NewGuid().ToString(),
                Email = signUpRequest.Email,
                LocalId = Guid.NewGuid().ToString()
            };

            //Setup defines what is going to happen when the method will be called
            authClientMock
                 .Setup(authClient => authClient
                 .SignUpUserAsync(signUpResponse.Email, signUpRequest.Password))
                 .ReturnsAsync(signUpResponse);
            // return response only when those exact passed values are provided


            //sut - system under test
            var sut = new UserService(userRepositoryMock.Object, authClientMock.Object);

            //Act - we call the method we want to test

            var result = await sut.SignUpAsync(signUpRequest);

            //Assert - did the required methods were called, did the intended data were returned

            authClientMock
                .Verify(authClient => authClient.SignUpUserAsync(It.Is<string>(value => value.Equals(signUpRequest.Email)),
                It.Is<string>(value => value.Equals(signUpRequest.Password))), Times.Once);

            /*            authClientMock
                .Verify(authClient => authClient.SignUpUserAsync(signUpRequest.Email, signUpRequest.Password), Times.Once);*/

            userRepositoryMock
                .Verify(userRepository => userRepository.CreateUserAysnc(It.Is<UserWriteModel>(user =>
                user.Email.Equals(signUpResponse.Email) &&
                user.LocalId.Equals(signUpResponse.LocalId))),
                Times.Once);

            Assert.Equal(signUpResponse.Email, result.Email);
            Assert.Equal(signUpResponse.LocalId, result.LocalId);
            Assert.IsType<Guid>(result.UserId);
        }
    }
}
