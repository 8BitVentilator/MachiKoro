using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class RanchTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceTotal.Of(2)), Category.Blue, Industry.Cow, Coins.Of(1));

        EstablishmentExpectation.From(new Ranch(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_ReturnsOneCoinFromBank()
    {
        Ranch ranch = new(TestEstablishmentId.Create());

        ranch.CreateIncomeClaim(Establishments.Empty).ShouldBe(new BankIncome(Coins.Of(1)));
    }
}
