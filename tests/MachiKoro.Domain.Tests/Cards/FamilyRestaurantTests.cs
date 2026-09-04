using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class FamilyRestaurantTests
{
    [Fact]
    public void Definition_ReturnsCardRules()
    {
        EstablishmentExpectation expected = new(
            ActivationNumbers.Between(DiceRoll.Of(9), DiceRoll.Of(10)),
            Category.Red,
            Industry.Cup,
            Coins.Of(3));

        EstablishmentExpectation.From(new FamilyRestaurant(TestEstablishmentId.Create())).ShouldBe(expected);
    }

    [Fact]
    public void CreateIncomeClaim_Always_TakesTwoCoinsFromActivePlayer()
    {
        FamilyRestaurant restaurant = new(TestEstablishmentId.Create());

        restaurant.CreateIncomeClaim(Establishments.Empty).ShouldBe(new ActivePlayerPayment(Coins.Of(2)));
    }
}
