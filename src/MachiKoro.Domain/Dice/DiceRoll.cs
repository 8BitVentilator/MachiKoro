namespace MachiKoro.Domain.Dice;

/// <summary>Represents a complete roll with one or two dice.</summary>
public sealed record DiceRoll
{
    private readonly DieResult _first;
    private readonly DieResult? _second;

    private DiceRoll(DieResult first, DieResult? second)
    {
        _first = first;
        _second = second;
    }

    /// <summary>Creates a roll with one die result.</summary>
    /// <param name="result">The die result.</param>
    /// <returns>The complete roll.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/> is <see langword="null"/>.</exception>
    public static DiceRoll Of(DieResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return new DiceRoll(result, null);
    }

    /// <summary>Creates a roll with two die results.</summary>
    /// <param name="first">The first die result.</param>
    /// <param name="second">The second die result.</param>
    /// <returns>The complete roll.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.
    /// </exception>
    public static DiceRoll Of(DieResult first, DieResult second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        return new DiceRoll(first, second);
    }

    /// <summary>Determines whether the roll has the specified total.</summary>
    /// <param name="total">The total to compare with.</param>
    /// <returns><see langword="true"/> when the totals match; otherwise, <see langword="false"/>.</returns>
    public bool HasTotal(DiceTotal total) => Total() == total;

    /// <summary>Determines whether the roll's total is in the inclusive specified range.</summary>
    /// <param name="first">The first total in the range.</param>
    /// <param name="last">The last total in the range.</param>
    /// <returns><see langword="true"/> when the total is in the range; otherwise, <see langword="false"/>.</returns>
    public bool HasTotalBetween(DiceTotal first, DiceTotal last) => Total().IsBetween(first, last);

    /// <summary>Determines whether the roll consists of two equal die results.</summary>
    /// <returns><see langword="true"/> when the roll is doubles; otherwise, <see langword="false"/>.</returns>
    public bool IsDoubles() => _second is DieResult second && _first == second;

    private DiceTotal Total()
    {
        if (_second is DieResult second)
        {
            return _first.Add(second);
        }

        return _first.AsTotal();
    }
}
