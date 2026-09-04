using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class CheeseFactoryTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.At(DiceRoll.Of(7)), Category.Green, Industry.Factory, Coins.Of(5));

        EstablishmentExpectation.From(new CheeseFactory(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_TwoCowEstablishments_ReturnsSixCoinsFromBank()
    {
        Establishments establishments = Establishments.Of(
            new Ranch(new EstablishmentId(Guid.Parse("81a1085a-b278-481c-810c-68ebbfce2c07"))),
            new Ranch(new EstablishmentId(Guid.Parse("6c798edd-9348-4cf9-bf9c-c5789b9135eb"))));
        CheeseFactory factory = new(TestEstablishmentId.Create());

        factory.CreateIncomeClaim(establishments).ShouldBe(new BankIncome(Coins.Of(6)));
    }
}
