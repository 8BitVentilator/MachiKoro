using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Apple Orchard establishment card.</summary>
public sealed class AppleOrchard : Establishment
{
    private static readonly FixedIncomeFromBank Effect = new(Coins.Of(3));

    /// <summary>Initializes an Apple Orchard card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public AppleOrchard(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(10));

    /// <inheritdoc/>
    public override Category Category => Category.Blue;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Wheat;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(3);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
