using System.Globalization;

namespace MachiKoro.Domain.Economy;

/// <summary>
/// Represents a nonnegative coin amount used for balances, income, and construction costs.
/// </summary>
public readonly record struct Coins
{
    /// <summary>Represents an amount of zero coins.</summary>
    public static readonly Coins Zero = new(0);

    private readonly int _amount;

    private Coins(int amount)
    {
        _amount = amount;
    }

    /// <summary>Creates a coin amount with the specified value.</summary>
    /// <param name="amount">The nonnegative number of coins.</param>
    /// <returns>The coin amount.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is negative.</exception>
    public static Coins Of(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return new Coins(amount);
    }

    /// <summary>Adds received income to this amount.</summary>
    /// <param name="income">The income to add.</param>
    /// <returns>The combined coin amount.</returns>
    /// <exception cref="OverflowException">The combined amount exceeds the supported range.</exception>
    public Coins Receive(Coins income) => new(checked(_amount + income._amount));

    /// <summary>Pays as much of a cost as possible without making the amount negative.</summary>
    /// <param name="cost">The requested payment.</param>
    /// <returns>The remaining coins, or zero when the cost exceeds this amount.</returns>
    public Coins Pay(Coins cost) => new(Math.Max(0, _amount - cost._amount));

    /// <summary>Determines whether this amount covers a cost.</summary>
    /// <param name="cost">The cost to evaluate.</param>
    /// <returns><see langword="true"/> when this amount covers the cost; otherwise, <see langword="false"/>.</returns>
    public bool CanAfford(Coins cost) => _amount >= cost._amount;

    /// <inheritdoc/>
    public override string ToString() => _amount.ToString(CultureInfo.InvariantCulture);
}
