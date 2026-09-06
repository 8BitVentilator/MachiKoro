using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents a collection of establishments owned by a player.</summary>
public sealed class Establishments
{
    /// <summary>Represents a collection with no establishments.</summary>
    public static readonly Establishments Empty = new([]);

    private readonly IReadOnlyCollection<Establishment> _items;

    private Establishments(IReadOnlyCollection<Establishment> items)
    {
        _items = items;
    }

    /// <summary>Creates a collection containing the specified establishments.</summary>
    /// <param name="establishments">The establishments to include.</param>
    /// <returns>The establishment collection.</returns>
    public static Establishments Of(params Establishment[] establishments) =>
        new(Array.AsReadOnly(establishments.ToArray()));

    /// <summary>Calculates income for every establishment in a specified industry.</summary>
    /// <param name="industry">The industry whose establishments produce income.</param>
    /// <param name="incomePerEstablishment">The income produced by each matching establishment.</param>
    /// <returns>The total income produced by the matching establishments.</returns>
    public Coins IncomeFor(Industry industry, Coins incomePerEstablishment) =>
        _items
            .Where(establishment => establishment.HasIndustry(industry))
            .Aggregate(Coins.Zero, (income, _) => income.Receive(incomePerEstablishment));
}
