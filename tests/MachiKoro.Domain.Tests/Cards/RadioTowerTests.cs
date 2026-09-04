using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class RadioTowerTests
{
    [Fact]
    public void Cost_ReturnsTwentyTwoCoins()
    {
        RadioTower tower = new(TestLandmarkId.Create());

        tower.Cost.ShouldBe(Coins.Of(22));
    }
}
