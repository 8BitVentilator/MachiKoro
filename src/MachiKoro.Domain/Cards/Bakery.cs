using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Bakery establishment card.</summary>
public sealed class Bakery : Establishment
{
    /// <summary>Initializes a Bakery card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public Bakery(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers =>
        ActivationNumbers.Between(DiceTotal.Of(2), DiceTotal.Of(3));

    /// <inheritdoc/>
    public override Category Category => Category.Green;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Shop;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(1);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => FixedIncomeFromBank.One;
}
