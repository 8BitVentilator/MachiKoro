using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class TrainStation : Landmark
{
    public TrainStation(LandmarkId id)
        : base(id)
    {
    }

    public override Coins Cost => Coins.Of(4);
}
