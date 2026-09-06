using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Family Restaurant establishment card.</summary>
public sealed class FamilyRestaurant : Establishment
{
    private static readonly TakeFromActivePlayer Effect = new(Coins.Of(2));

    /// <summary>Initializes a Family Restaurant card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public FamilyRestaurant(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers =>
        ActivationNumbers.Between(DiceTotal.Of(9), DiceTotal.Of(10));

    /// <inheritdoc/>
    public override Category Category => Category.Red;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Cup;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(3);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
