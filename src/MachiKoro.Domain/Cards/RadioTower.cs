using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class RadioTower : Landmark
{
    public RadioTower(LandmarkId id)
        : base(id)
    {
    }

    public override Coins Cost => Coins.Of(22);
}
