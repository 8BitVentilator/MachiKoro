using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class WheatFieldTests
{
    [Fact]
    public void Id_DifferentCopies_IsIndividual()
    {
        WheatField first = new(new EstablishmentId(Guid.Parse("27a0438a-9c33-43ba-877c-881342bd6086")));
        WheatField second = new(new EstablishmentId(Guid.Parse("af00e053-91e1-44c9-85da-04b5269dac30")));

        first.Id.ShouldNotBe(second.Id);
    }

    [Fact]
    public void IsActivatedBy_One_ReturnsTrue()
    {
        WheatField wheatField = CreateWheatField();

        wheatField.IsActivatedBy(DiceRoll.Of(1)).ShouldBeTrue();
    }

    [Fact]
    public void IsActivatedBy_Two_ReturnsFalse()
    {
        WheatField wheatField = CreateWheatField();

        wheatField.IsActivatedBy(DiceRoll.Of(2)).ShouldBeFalse();
    }

    [Fact]
    public void Category_ReturnsBlue()
    {
        CreateWheatField().Category.ShouldBe(Category.Blue);
    }

    [Fact]
    public void Industry_ReturnsWheat()
    {
        CreateWheatField().Industry.ShouldBe(Industry.Wheat);
    }

    [Fact]
    public void Cost_ReturnsOneCoin()
    {
        CreateWheatField().Cost.ShouldBe(Coins.Of(1));
    }

    [Fact]
    public void CreateIncomeClaim_Always_ReturnsOneCoinFromBank()
    {
        BankIncome expected = new(Coins.Of(1));

        CreateWheatField().CreateIncomeClaim(Establishments.Empty).ShouldBe(expected);
    }

    private static WheatField CreateWheatField() =>
        new(new EstablishmentId(Guid.Parse("860675c5-e78a-4cf0-a2cb-25cce10b0f76")));
}
