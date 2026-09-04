using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public abstract class Landmark
{
    protected Landmark(LandmarkId id)
    {
        Id = id;
    }

    public LandmarkId Id { get; }

    public abstract Coins Cost { get; }
}
