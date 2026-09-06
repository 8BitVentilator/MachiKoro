using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Convenience Store establishment card.</summary>
public sealed class ConvenienceStore : Establishment
{
    private static readonly FixedIncomeFromBank Effect = new(Coins.Of(3));

    /// <summary>Initializes a Convenience Store card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public ConvenienceStore(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(4));

    /// <inheritdoc/>
    public override Category Category => Category.Green;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Shop;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(2);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
