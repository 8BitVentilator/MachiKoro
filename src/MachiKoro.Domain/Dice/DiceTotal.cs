namespace MachiKoro.Domain.Dice;

/// <summary>Represents the total of a roll with one or two dice.</summary>
public sealed record DiceTotal
{
    private const int Minimum = 1;
    private const int Maximum = 12;
    private readonly int _value;

    private DiceTotal(int value)
    {
        _value = value;
    }

    /// <summary>Creates a dice total with the specified value.</summary>
    /// <param name="value">The total value.</param>
    /// <returns>The dice total.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is less than one or greater than twelve.
    /// </exception>
    public static DiceTotal Of(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, Minimum);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Maximum);
        return new DiceTotal(value);
    }

    /// <summary>Determines whether this total is in the inclusive specified range.</summary>
    /// <param name="first">The first total in the range.</param>
    /// <param name="last">The last total in the range.</param>
    /// <returns><see langword="true"/> when this total is in the range; otherwise, <see langword="false"/>.</returns>
    public bool IsBetween(DiceTotal first, DiceTotal last) =>
        _value >= first._value && _value <= last._value;

    /// <summary>Determines whether this total precedes another total.</summary>
    /// <param name="other">The total to compare with.</param>
    /// <returns><see langword="true"/> when this total is smaller; otherwise, <see langword="false"/>.</returns>
    public bool IsBefore(DiceTotal other) => _value < other._value;
}
