using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Cafe establishment card.</summary>
public sealed class Cafe : Establishment
{
    private static readonly TakeFromActivePlayer Effect = new(Coins.Of(1));

    /// <summary>Initializes a Cafe card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public Cafe(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(3));

    /// <inheritdoc/>
    public override Category Category => Category.Red;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Cup;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(2);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
