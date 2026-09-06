using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the Stadium establishment card.</summary>
public sealed class Stadium : Establishment
{
    private static readonly TakeFromEveryone Effect = new(Coins.Of(2));

    /// <summary>Initializes a Stadium card.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    public Stadium(EstablishmentId id)
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
    public override Coins Cost => Coins.Of(6);

    /// <inheritdoc/>
    protected override IIncomeEffect IncomeEffect => Effect;
}
