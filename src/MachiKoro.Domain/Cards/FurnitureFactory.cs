using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Furniture Factory establishment card.</summary>
public sealed class FurnitureFactory : Establishment
{
    private static readonly IncomePerIndustry Effect = new(Industry.Gear, Coins.Of(3));

    /// <summary>Initializes a Furniture Factory card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public FurnitureFactory(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(8));

    /// <inheritdoc/>
    public override Category Category => Category.Green;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Factory;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(3);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
