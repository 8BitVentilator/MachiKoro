using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class ConvenienceStoreTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceTotal.Of(4)), Category.Green, Industry.Shop, Coins.Of(2));

        EstablishmentExpectation.From(new ConvenienceStore(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_ReturnsThreeCoinsFromBank()
    {
        ConvenienceStore store = new(TestEstablishmentId.Create());

        store.CreateIncomeClaim(Establishments.Empty).ShouldBe(new BankIncome(Coins.Of(3)));
    }
}
