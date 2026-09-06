using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Ranch establishment card.</summary>
public sealed class Ranch : Establishment
{
    /// <summary>Initializes a Ranch card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public Ranch(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(2));

    /// <inheritdoc/>
    public override Category Category => Category.Blue;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Cow;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(1);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => FixedIncomeFromBank.One;
}
