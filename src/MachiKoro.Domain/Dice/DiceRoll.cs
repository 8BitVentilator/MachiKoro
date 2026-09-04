using System.Runtime.InteropServices;

namespace MachiKoro.Domain.Dice;

[StructLayout(LayoutKind.Auto)]
public readonly record struct DiceRoll
{
    private const int Minimum = 1;
    private const int Maximum = 12;
    private readonly int _value;

    private DiceRoll(int value)
    {
        _value = value;
    }

    public static DiceRoll Of(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, Minimum);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Maximum);
        return new DiceRoll(value);
    }

    public bool IsBetween(DiceRoll first, DiceRoll last) =>
        _value >= first._value && _value <= last._value;

    public bool IsBefore(DiceRoll other) => _value < other._value;
}
