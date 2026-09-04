using System.Runtime.InteropServices;
using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Cards;

[StructLayout(LayoutKind.Auto)]
public readonly record struct ActivationNumbers
{
    private readonly DiceRoll _first;
    private readonly DiceRoll _last;

    private ActivationNumbers(DiceRoll first, DiceRoll last)
    {
        _first = first;
        _last = last;
    }

    public static ActivationNumbers At(DiceRoll number) => new(number, number);

    public static ActivationNumbers Between(DiceRoll first, DiceRoll last)
    {
        if (last.IsBefore(first))
        {
            throw new ArgumentOutOfRangeException(nameof(last));
        }

        return new ActivationNumbers(first, last);
    }

    public bool Includes(DiceRoll roll) => roll.IsBetween(_first, _last);
}
