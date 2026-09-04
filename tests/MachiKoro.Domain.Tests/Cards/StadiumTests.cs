using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class StadiumTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceRoll.Of(6)), Category.Purple, Industry.Tower, Coins.Of(6));

        EstablishmentExpectation.From(new Stadium(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_TakesTwoCoinsFromEveryPlayer()
    {
        Stadium stadium = new(TestEstablishmentId.Create());

        stadium.CreateIncomeClaim(Establishments.Empty).ShouldBe(new EveryPlayerPayment(Coins.Of(2)));
    }
}
