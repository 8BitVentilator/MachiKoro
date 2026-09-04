using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class TelevisionStationTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceRoll.Of(6)), Category.Purple, Industry.Tower, Coins.Of(7));

        EstablishmentExpectation.From(new TelevisionStation(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_TakesFiveCoinsFromChosenPlayer()
    {
        TelevisionStation station = new(TestEstablishmentId.Create());

        station.CreateIncomeClaim(Establishments.Empty).ShouldBe(new ChosenPlayerPayment(Coins.Of(5)));
    }
}
