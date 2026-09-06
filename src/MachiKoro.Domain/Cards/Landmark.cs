using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Defines a landmark card that a player can construct.</summary>
public abstract class Landmark
{
    /// <summary>Initializes a landmark with its identifier.</summary>
    /// <param name="id">The identifier of the landmark card.</param>
    protected Landmark(LandmarkId id)
    {
        Id = id;
    }

    /// <summary>Gets the identifier of this landmark card.</summary>
    public LandmarkId Id { get; }

    /// <summary>Gets the construction cost of this landmark.</summary>
    public abstract Coins Cost { get; }
}
