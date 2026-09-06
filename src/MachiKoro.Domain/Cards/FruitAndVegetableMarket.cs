using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Fruit and Vegetable Market establishment card.</summary>
public sealed class FruitAndVegetableMarket : Establishment
{
    private static readonly IncomePerIndustry Effect = new(Industry.Wheat, Coins.Of(2));

    /// <summary>Initializes a Fruit and Vegetable Market card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public FruitAndVegetableMarket(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers =>
        ActivationNumbers.Between(DiceTotal.Of(11), DiceTotal.Of(12));

    /// <inheritdoc/>
    public override Category Category => Category.Green;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Fruit;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(2);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
