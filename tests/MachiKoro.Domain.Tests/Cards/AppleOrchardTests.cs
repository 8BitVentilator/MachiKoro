using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class AppleOrchardTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceRoll.Of(10)), Category.Blue, Industry.Wheat, Coins.Of(3));

        EstablishmentExpectation.From(new AppleOrchard(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_ReturnsThreeCoinsFromBank()
    {
        AppleOrchard orchard = new(TestEstablishmentId.Create());

        orchard.CreateIncomeClaim(Establishments.Empty).ShouldBe(new BankIncome(Coins.Of(3)));
    }
}
