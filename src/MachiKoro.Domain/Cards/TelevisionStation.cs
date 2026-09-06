using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Television Station establishment card.</summary>
public sealed class TelevisionStation : Establishment
{
    private static readonly TakeFromChosenPlayer Effect = new(Coins.Of(5));

    /// <summary>Initializes a Television Station card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public TelevisionStation(EstablishmentId id)
        : base(id)
    {
    }

    /// <inheritdoc/>
    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(6));

    /// <inheritdoc/>
    public override Category Category => Category.Purple;

    /// <inheritdoc/>
    public override Industry Industry => Industry.Tower;

    /// <inheritdoc/>
    public override Coins Cost => Coins.Of(7);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
