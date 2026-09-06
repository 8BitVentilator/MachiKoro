using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Train Station landmark card.</summary>
public sealed class TrainStation : Landmark
{
    /// <summary>Initializes a Train Station card.</summary>
    /// <param name="id">The identifier of the landmark card.</param>
    public TrainStation(LandmarkId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(4);
}
