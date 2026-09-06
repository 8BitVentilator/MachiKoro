using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Wheat Field establishment card.</summary>
public sealed class WheatField : Establishment
{
    /// <summary>Initializes a Wheat Field card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public WheatField(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(1));

    /// <inheritdoc/>
    public override Category Category => Category.Blue;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Wheat;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(1);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => FixedIncomeFromBank.One;
}
