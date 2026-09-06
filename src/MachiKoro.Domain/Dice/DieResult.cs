namespace MachiKoro.Domain.Dice;

/// <summary>Represents the result of one six-sided die.</summary>
public sealed record DieResult
{
    private const int Minimum = 1;
    private const int Maximum = 6;
    private readonly int _value;

    private DieResult(int value)
    {
        _value = value;
    }

    /// <summary>Creates a result with the specified number of pips.</summary>
    /// <param name="value">The number of pips.</param>
    /// <returns>The die result.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is less than one or greater than six.
    /// </exception>
    public static DieResult Of(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, Minimum);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Maximum);
        return new DieResult(value);
    }

    internal DiceTotal AsTotal() => DiceTotal.Of(_value);

    internal DiceTotal Add(DieResult other) => DiceTotal.Of(_value + other._value);
}
