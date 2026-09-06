using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class FurnitureFactoryTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceTotal.Of(8)), Category.Green, Industry.Factory, Coins.Of(3));

        EstablishmentExpectation.From(new FurnitureFactory(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_TwoGearEstablishments_ReturnsSixCoinsFromBank()
    {
        Establishments establishments = Establishments.Of(
            new Forest(new EstablishmentId(Guid.Parse("05e0fa64-d277-4c66-a949-e9b1a3c8138a"))),
            new Mine(new EstablishmentId(Guid.Parse("09e3bba3-79ff-4891-b13e-308eb39c3579"))));
        FurnitureFactory factory = new(TestEstablishmentId.Create());

        factory.CreateIncomeClaim(establishments).ShouldBe(new BankIncome(Coins.Of(6)));
    }
}
