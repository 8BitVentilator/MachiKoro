using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class CafeTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceRoll.Of(3)), Category.Red, Industry.Cup, Coins.Of(2));

        EstablishmentExpectation.From(new Cafe(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_TakesOneCoinFromActivePlayer()
    {
        Cafe cafe = new(TestEstablishmentId.Create());

        cafe.CreateIncomeClaim(Establishments.Empty).ShouldBe(new ActivePlayerPayment(Coins.Of(1)));
    }
}
