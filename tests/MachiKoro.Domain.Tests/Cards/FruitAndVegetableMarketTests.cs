using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class FruitAndVegetableMarketTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.Between(DiceTotal.Of(11), DiceTotal.Of(12)),
            Category.Green,
            Industry.Fruit,
            Coins.Of(2));

        EstablishmentExpectation.From(new FruitAndVegetableMarket(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_TwoWheatEstablishments_ReturnsFourCoinsFromBank()
    {
        Establishments establishments = Establishments.Of(
            new WheatField(new EstablishmentId(Guid.Parse("d2587d56-2609-4b08-b366-4a92d57a2794"))),
            new AppleOrchard(new EstablishmentId(Guid.Parse("519e3982-041e-4e20-95ca-510f78cf52e5"))));
        FruitAndVegetableMarket market = new(TestEstablishmentId.Create());

        market.CreateIncomeClaim(establishments).ShouldBe(new BankIncome(Coins.Of(4)));
    }
}
