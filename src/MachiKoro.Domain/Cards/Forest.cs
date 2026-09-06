using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Forest establishment card.</summary>
public sealed class Forest : Establishment
{
    /// <summary>Initializes a Forest card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public Forest(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(5));

    /// <inheritdoc/>
    public override Category Category => Category.Blue;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Gear;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(3);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => FixedIncomeFromBank.One;
}
