using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class ForestTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceRoll.Of(5)), Category.Blue, Industry.Gear, Coins.Of(3));

        EstablishmentExpectation.From(new Forest(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_ReturnsOneCoinFromBank()
    {
        Forest forest = new(TestEstablishmentId.Create());

        forest.CreateIncomeClaim(Establishments.Empty).ShouldBe(new BankIncome(Coins.Of(1)));
    }
}
