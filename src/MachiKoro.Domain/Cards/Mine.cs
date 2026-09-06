using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Mine establishment card.</summary>
public sealed class Mine : Establishment
{
    private static readonly FixedIncomeFromBank Effect = new(Coins.Of(5));

    /// <summary>Initializes a Mine card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public Mine(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(9));

    /// <inheritdoc/>
    public override Category Category => Category.Blue;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Gear;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(6);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
