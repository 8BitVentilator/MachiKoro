using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class BakeryTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.Between(DiceRoll.Of(2), DiceRoll.Of(3)),
            Category.Green,
            Industry.Shop,
            Coins.Of(1));

        EstablishmentExpectation.From(new Bakery(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_ReturnsOneCoinFromBank()
    {
        Bakery bakery = new(TestEstablishmentId.Create());

        bakery.CreateIncomeClaim(Establishments.Empty).ShouldBe(new BankIncome(Coins.Of(1)));
    }
}
