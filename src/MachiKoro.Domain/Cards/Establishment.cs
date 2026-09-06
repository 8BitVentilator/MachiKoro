using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Defines an establishment card and the income claim it creates when activated.</summary>
public abstract class Establishment
{
    /// <summary>Initializes an establishment with its identifier.</summary>
    /// <param name="id">The identifier of the establishment card.</param>
    protected Establishment(EstablishmentId id)
    {
        Id = id;
    }

    /// <summary>Gets the identifier of this establishment card.</summary>
    public EstablishmentId Id { get; }

    /// <summary>Gets the dice totals that activate this establishment.</summary>
    public abstract ActivationNumbers ActivationNumbers { get; }

    /// <summary>Gets the income category of this establishment.</summary>
    public abstract Category Category { get; }

    /// <summary>Gets the industry symbol of this establishment.</summary>
    public abstract Industry Industry { get; }

    /// <summary>Gets the construction cost of this establishment.</summary>
    public abstract Coins Cost { get; }

    /// <summary>Gets the income effect applied when this establishment is activated.</summary>
    protected abstract IIncomeEffect IncomeEffect { get; }

    /// <summary>Determines whether a dice roll activates this establishment.</summary>
    /// <param name="roll">The dice roll to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> when the roll activates this establishment; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsActivatedBy(DiceRoll roll) => ActivationNumbers.Includes(roll);

    /// <summary>Determines whether this establishment belongs to an industry.</summary>
    /// <param name="industry">The industry to compare with.</param>
    /// <returns><see langword="true"/> when the industries match; otherwise, <see langword="false"/>.</returns>
    public bool HasIndustry(Industry industry) => Industry == industry;

    /// <summary>Creates the income claim produced by this establishment.</summary>
    /// <param name="establishments">The establishments available when calculating the claim.</param>
    /// <returns>The income claim produced by this establishment.</returns>
    public IncomeClaim CreateIncomeClaim(Establishments establishments) =>
        IncomeEffect.CreateClaim(establishments);
}
