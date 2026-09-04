using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class ShoppingMallTests
{
    [Fact]
    public void Cost_ReturnsTenCoins()
    {
        ShoppingMall mall = new(TestLandmarkId.Create());

        mall.Cost.ShouldBe(Coins.Of(10));
    }
}
