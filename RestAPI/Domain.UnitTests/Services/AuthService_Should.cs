using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Contracts.RequestModels;
using Contracts.ResponseModels;
using Domain.Client;
using Domain.Client.Models.RequestModels;
using Domain.Client.Models.ResponseModels;
using Domain.Models.RequestModels;
using Domain.Services;
using Domain.UnitTests.Attributes;
using FluentAssertions;
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
        [Theory, AutoMoqData]
        public async Task SignInAsync_WithSignInRequest_ReturnSignInResponse(
            SignInRequest signInRequest,
            ClientSignInUserResponse signInResponse,
            UserReadModel userReadModel,
            [Frozen] Mock<IAuthClient> authClientMock,
            [Frozen] Mock<IUsersRepository> userRepositoryMock,
            UserService sut)
        {
            //Arange - preparation of data, creation of mock data model objects
            signInResponse.Email = signInRequest.Email;

            userReadModel.LocalId = signInResponse.LocalId;
            userReadModel.Email = signInResponse.Email;

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
        //[Fact]
        [Theory] // allows to define insertion models into method
        [AutoMoqData]
        public async Task SignUpAsync_WithSignUpRequest_ReturnSignUpResponse(
            SignUpRequest signUpRequest,
            CreateUserResponse signUpResponse,
            [Frozen] Mock<IAuthClient> authClientMock,  //every object (Mock) that needs to be inserted into 'sut', it has to be attributed with [Frozen]
            [Frozen] Mock<IUsersRepository> userRepositoryMock,
            UserService sut)
        {

            //AS WE HAVE CUSTOMZIED AND AutoDat WITH AutoMoqData
            /*            var fixture = new Fixture().Customize(new AutoMoqCustomization());

                        var authClientMock = fixture.Freeze<Mock<IAuthClient>>();
                        var userRepositoryMock = fixture.Freeze<Mock<IUsersRepository>>();

                        var sut = fixture.Create<UserService>();*/

            //Arange - preparation of data, creation of mock data model objects
            /*            var authClientMock = new Mock<IAuthClient>();
                        var userRepositoryMock = new Mock<IUsersRepository>();
            */
            //SIMPLIFIED:

            /*            var fixture = new Fixture();

                        var authClientMock = fixture.Create<Mock<IAuthClient>>();

                        var userRepositoryMock = fixture.Create<Mock<IUsersRepository>>();*/

            //EVEN MORE SIMPLIFIED -> USE OF [AutoData] AND [Theory] -> 'var fixture = new Fixture();' NOT NEEDED ANYMORE


            // expected input - data that would be provided to the tested method (in this case AuthService method SignIn)
            /*var signUpRequest = new SignUpRequest
            {
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };*/

            //SIMPLIFIED with 'new Fixture():

            //var signUpRequest = fixture.Create<SignUpRequest>();

            //EVEN MORE SIMPLIFIED -> USE OF [AutoData] AND [Theory]

            //expected output from authClient
            /*            var signUpResponse = new CreateUserResponse
                        {
                            IdToken = Guid.NewGuid().ToString(),
                            Email = signUpRequest.Email,
                            LocalId = Guid.NewGuid().ToString()
                        };*/

            //SIMPLIFIED with 'new Fixture():

            //var fixture = new Fixture();

            //var signUpResponse = fixture.Create<CreateUserResponse>();

            //EVEN MORE SIMPLIFIED -> USE OF [AutoData] AND [Theory] -> 'var fixture = new Fixture();' NOT NEEDED ANYMORE

            signUpResponse.Email = signUpRequest.Email;

            //Setup defines what is going to happen when the method will be called
            authClientMock
                 .Setup(authClient => authClient
                 .SignUpUserAsync(signUpResponse.Email, signUpRequest.Password))
                 .ReturnsAsync(signUpResponse);
            // return response only when those exact passed values are provided


            //sut - system under test
            //var sut = new UserService(userRepositoryMock.Object, authClientMock.Object);
            //EVEN MORE SIMPLIFIED -> USE OF [AutoData] AND [Theory] AND [Frozen]. Implemented with fixture.Customize

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

            result.Should().BeEquivalentTo(result, options => options.ComparingByMembers<UserReadModel>());

            result.Email.Should().BeEquivalentTo(signUpResponse.Email);
            result.LocalId.Should().BeEquivalentTo(signUpResponse.LocalId);
            result.DateCreated.GetType().Should().Be<DateTime>();

        }
    }
}
