using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class AmusementPark : Landmark
{
    public AmusementPark(LandmarkId id)
        : base(id)
    {
    }

    public override Coins Cost => Coins.Of(16);
}
