using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class AmusementParkTests
{
    [Fact]
    public void Cost_ReturnsSixteenCoins()
    {
        AmusementPark park = new(TestLandmarkId.Create());

        park.Cost.ShouldBe(Coins.Of(16));
    }
}
