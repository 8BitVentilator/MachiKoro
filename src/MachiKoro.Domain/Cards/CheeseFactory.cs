using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Cheese Factory establishment card.</summary>
public sealed class CheeseFactory : Establishment
{
    private static readonly IncomePerIndustry Effect = new(Industry.Cow, Coins.Of(3));

    /// <summary>Initializes a Cheese Factory card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public CheeseFactory(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(7));

    /// <inheritdoc/>
    public override Category Category => Category.Green;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Factory;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(5);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
