using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Shopping Mall landmark card.</summary>
public sealed class ShoppingMall : Landmark
{
    /// <summary>Initializes a Shopping Mall card.</summary>
    /// <param name="id">The identifier of the landmark card.</param>
    public ShoppingMall(LandmarkId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(10);
}
