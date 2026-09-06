using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Amusement Park landmark card.</summary>
public sealed class AmusementPark : Landmark
{
    /// <summary>Initializes an Amusement Park card.</summary>
    /// <param name="id">The identifier of the landmark card.</param>
    public AmusementPark(LandmarkId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(16);
}
