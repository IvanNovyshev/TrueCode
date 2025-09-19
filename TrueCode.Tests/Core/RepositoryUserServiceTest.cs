using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TrueCode.UserService.Core;

namespace TrueCode.FinanceService.Tests.Core;

[TestFixture]
[TestOf(typeof(RepositoryUserService))]
public class RepositoryUserServiceTest
{
    [Test]
    [AutoMoq]
    public async Task RepositoryUserService_CreateUser_ShouldHashPassword(
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        [Frozen] Mock<IHashCreator<string>> hashStub, string hashResult, CreateUserCommand command,
        RepositoryUserService sut)
    {
        //Arrange
        hashStub.Setup(x => x.CreateHash(It.Is<string>(match => match.Equals(command.Password)))).Returns(hashResult);

        //Act
        await sut.CreateUserAsync(command);

        //Assert
        userRepositoryMock.Verify(x => x.AddUserAsync(It.Is<NewUser>(user => user.Hash.Equals(hashResult))));
    }

    [Test]
    [AutoMoq]
    public async Task RepositoryUserService_RepositoryAlreadyContainThatUser_ShouldReThrowException(
        [Frozen] Mock<IUserRepository> userRepositoryStub,
        [Frozen] Mock<IHashCreator<string>> hashStub, string hashResult, CreateUserCommand command,
        RepositoryUserService sut)
    {
        //Arrange
        userRepositoryStub.Setup(x => x.AddUserAsync(It.IsAny<NewUser>()))
            .ThrowsAsync(new UserAlreadyExistsException("any") { Name = command.Name });

        //Act
        var act = async () => await sut.CreateUserAsync(command);

        //Assert
        await act.Should().ThrowExactlyAsync<UserAlreadyExistsException>();
    }

    [Test]
    [AutoMoq]
    public async Task RepositoryUserService_RepositoryThrowException_ShouldReThrowException(
        [Frozen] Mock<IUserRepository> userRepositoryStub,
        [Frozen] Mock<IHashCreator<string>> hashStub, string hashResult, CreateUserCommand command,
        RepositoryUserService sut)
    {
        //Arrange
        userRepositoryStub.Setup(x => x.AddUserAsync(It.IsAny<NewUser>()))
            .ThrowsAsync(new Exception());

        //Act
        var act = async () => await sut.CreateUserAsync(command);

        //Assert
        await act.Should().ThrowExactlyAsync<CreateUserException>();
    }
}