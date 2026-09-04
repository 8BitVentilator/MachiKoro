using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class ShoppingMall : Landmark
{
    public ShoppingMall(LandmarkId id)
        : base(id)
    {
    }

    public override Coins Cost => Coins.Of(10);
}
