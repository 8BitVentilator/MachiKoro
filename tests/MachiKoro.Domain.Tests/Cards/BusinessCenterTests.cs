using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class BusinessCenterTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceRoll.Of(6)), Category.Purple, Industry.Tower, Coins.Of(8));

        EstablishmentExpectation.From(new BusinessCenter(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_RequestsEstablishmentSwap()
    {
        BusinessCenter center = new(TestEstablishmentId.Create());

        center.CreateIncomeClaim(Establishments.Empty).ShouldBe(new EstablishmentSwap());
    }
}
