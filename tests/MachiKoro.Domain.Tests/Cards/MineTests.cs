using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class MineTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceTotal.Of(9)), Category.Blue, Industry.Gear, Coins.Of(6));

        EstablishmentExpectation.From(new Mine(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_ReturnsFiveCoinsFromBank()
    {
        Mine mine = new(TestEstablishmentId.Create());

        mine.CreateIncomeClaim(Establishments.Empty).ShouldBe(new BankIncome(Coins.Of(5)));
    }
}
