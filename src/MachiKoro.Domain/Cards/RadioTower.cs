using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Radio Tower landmark card.</summary>
public sealed class RadioTower : Landmark
{
    /// <summary>Initializes a Radio Tower card.</summary>
    /// <param name="id">The identifier of the landmark card.</param>
    public RadioTower(LandmarkId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(22);
}
