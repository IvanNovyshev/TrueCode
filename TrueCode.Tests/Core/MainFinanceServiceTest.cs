using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TrueCode.FinanceService.Core;

namespace TrueCode.FinanceService.Tests.Core;

[TestFixture]
[TestOf(typeof(MainFinanceService))]
public class MainFinanceServiceTest
{
    [Test]
    [AutoMoq]
    public async Task SetFavoritesCodesForUserAsync_AddNewFavoriteCodes_ShouldAlwaysSaveChanges(
        [Frozen] Mock<IUserFavoriteCodesRepository> favoriteCodes,
        [Frozen] Mock<ICurrencyRatesRepository> ratesRepository,
        SetFavoritesCommand command,
        MainFinanceService sut)
    {
        //Assert
        //Act
        await sut.SetFavoritesCodesForUserAsync(command);

        //Arrange
        favoriteCodes.Verify(x => x.SaveAsync());
    }

    [Test]
    [AutoMoq]
    public async Task SetFavoritesCodesForUserAsync_AddNewFavoriteCodes_ShouldAlwaysDeleteOwnFavoriteCodes(
        [Frozen] Mock<IUserFavoriteCodesRepository> favoriteCodes,
        SetFavoritesCommand command,
        MainFinanceService sut)
    {
        //Assert
        //Act
        await sut.SetFavoritesCodesForUserAsync(command);

        //Arrange
        favoriteCodes.Verify(x => x.RemoveFavoriteCodesByUserNameAsync(It.Is<string>(s => s.Equals(command.Name))));
    }

    [Test]
    [AutoMoq]
    public async Task SetFavoritesCodesForUserAsync_AddNewFavoriteCodes_ShouldAlwaysAddNewFavoriteCodes(
        [Frozen] Mock<IUserFavoriteCodesRepository> favoriteCodes,
        SetFavoritesCommand command,
        MainFinanceService sut)
    {
        //Assert
        //Act
        await sut.SetFavoritesCodesForUserAsync(command);

        //Arrange
        favoriteCodes.Verify(x => x.AddFavoriteCodesForUserAsync(It.Is<string>(s => s.Equals(command.Name)),
            It.Is<IReadOnlyCollection<string>>(collection => collection.SequenceEqual(command.Codes))));
    }

    [Test]
    [AutoMoq]
    public async Task SetFavoritesCodesForUserAsync_SaveAsyncThrowAnException_ShouldThrowServiceException(
        [Frozen] Mock<IUserFavoriteCodesRepository> favoriteCodes,
        SetFavoritesCommand command,
        MainFinanceService sut)
    {
        //Assert
        favoriteCodes.Setup(x => x.SaveAsync()).Throws(new Exception());

        //Act
        var task = async () => await sut.SetFavoritesCodesForUserAsync(command);

        //Arrange
        await task.Should().ThrowExactlyAsync<FinanceServiceException>();
    }

    [Test]
    [AutoMoq]
    public async Task SetFavoritesCodesForUserAsync_RemoveFavoritesThrowAnException_ShouldNotCallSaveChanges(
        [Frozen] Mock<IUserFavoriteCodesRepository> favoriteCodes,
        SetFavoritesCommand command,
        MainFinanceService sut)
    {
        //Assert
        favoriteCodes.Setup(x => x.RemoveFavoriteCodesByUserNameAsync(It.IsAny<string>())).Throws(new Exception());

        //Act
        await sut.SetFavoritesCodesForUserAsync(command).ContinueWith(x => x); // swallow exception

        //Arrange
        favoriteCodes.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Test]
    [AutoMoq]
    public async Task SetFavoritesCodesForUserAsync_AddFavoriteCodesThrowAnException_ShouldNotCallSaveChanges(
        [Frozen] Mock<IUserFavoriteCodesRepository> favoriteCodes,
        SetFavoritesCommand command,
        MainFinanceService sut)
    {
        //Assert
        favoriteCodes
            .Setup(x => x.AddFavoriteCodesForUserAsync(It.IsAny<string>(), It.IsAny<IReadOnlyCollection<string>>()))
            .Throws(new Exception());

        //Act
        await sut.SetFavoritesCodesForUserAsync(command).ContinueWith(x => x); // swallow exception

        //Arrange
        favoriteCodes.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Test]
    [AutoMoq]
    public async Task GetRatesForUserAsync_GetRatesByCodesThrowAnException_ShouldNotCallSaveChanges(
        [Frozen] Mock<IUserFavoriteCodesRepository> favoriteCodes,
        [Frozen] Mock<ICurrencyRatesRepository> ratesRepository,
        string name,
        MainFinanceService sut)
    {
        //Assert
        ratesRepository
            .Setup(x => x.GetRatesByCodes(It.IsAny<IQueryable<string>>()))
            .Throws(new Exception());

        //Act
        var act = async () => await sut.GetRatesForUserAsync(name);

        //Arrange
        await act.Should().ThrowExactlyAsync<FinanceServiceException>();
    }
}

public class AutoMoq : AutoDataAttribute
{
    public AutoMoq() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new AutoMoqCustomization());
        return fixture;
    })
    {
    }
}