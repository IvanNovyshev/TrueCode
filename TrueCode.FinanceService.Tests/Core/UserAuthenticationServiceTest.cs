using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TrueCode.UserService.Core;

namespace TrueCode.FinanceService.Tests.Core;

[TestFixture]
[TestOf(typeof(UserAuthenticationService))]
public class UserAuthenticationServiceTest
{
//  public UserAuthenticationService(IAccessTokenCreator<User> accessTokenCreator, IUserRepository repository)
    [Test]
    [AutoMoq]
    public async Task TryGetTokenAsync_RepositoryIsEmpty_ShouldReturnFailedResult(
        [Frozen] Mock<IAccessTokenCreator<User>> tokenCreatorStub,
        [Frozen] Mock<IUserRepository> userRepositoryStub,
        UserLoginCommand command,
        UserAuthenticationService sut)
    {
        //Arrange
        userRepositoryStub.Setup(x => x.GetUserByNameOrDefaultAsync(It.IsAny<string>())).ReturnsAsync((User)null);

        //Act
        var result = await sut.TryGetTokenAsync(command);

        //Assert
        result.IsSuccess.Should().Be(false);
    }

    [Test]
    [AutoMoq]
    public async Task TryGetTokenAsync_RepositoryContainsData_ShouldReturnSuccessResult(
        [Frozen] Mock<IAccessTokenCreator<User>> tokenCreatorStub,
        [Frozen] Mock<IUserRepository> userRepositoryStub,
        User someUser,
        UserLoginCommand command,
        UserAuthenticationService sut)
    {
        //Arrange
        userRepositoryStub.Setup(x => x.GetUserByNameOrDefaultAsync(It.IsAny<string>())).ReturnsAsync(someUser);

        //Act
        var result = await sut.TryGetTokenAsync(command);

        //Assert
        result.IsSuccess.Should().Be(true);
    }

    [Test]
    [AutoMoq]
    public async Task TryGetTokenAsync_RepositoryContainsData_ShouldReturnResultFromTokenCreator(
        [Frozen] Mock<IAccessTokenCreator<User>> tokenCreatorStub,
        [Frozen] Mock<IUserRepository> userRepositoryStub,
        User someUser,
        string expectedResult,
        UserLoginCommand command,
        UserAuthenticationService sut)
    {
        //Arrange
        userRepositoryStub.Setup(x => x.GetUserByNameOrDefaultAsync(It.IsAny<string>())).ReturnsAsync(someUser);
        tokenCreatorStub.Setup(x => x.CreateToken(It.IsAny<User>())).Returns(expectedResult);
        
        //Act
        var result = await sut.TryGetTokenAsync(command);

        //Assert
        result.Value.Should().Be(expectedResult);
    }
}