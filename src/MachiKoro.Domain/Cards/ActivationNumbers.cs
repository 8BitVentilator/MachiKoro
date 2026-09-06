using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Cards;

/// <summary>Represents the totals that activate an establishment.</summary>
public sealed record ActivationNumbers
{
    private readonly DiceTotal _first;
    private readonly DiceTotal _last;

    private ActivationNumbers(DiceTotal first, DiceTotal last)
    {
        _first = first;
        _last = last;
    }

    /// <summary>Creates one activation total.</summary>
    /// <param name="number">The activating total.</param>
    /// <returns>The activation numbers.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="number"/> is <see langword="null"/>.</exception>
    public static ActivationNumbers At(DiceTotal number)
    {
        ArgumentNullException.ThrowIfNull(number);
        return new ActivationNumbers(number, number);
    }

    /// <summary>Creates an inclusive range of activation totals.</summary>
    /// <param name="first">The first activating total.</param>
    /// <param name="last">The last activating total.</param>
    /// <returns>The activation numbers.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="first"/> or <paramref name="last"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="last"/> precedes <paramref name="first"/>.
    /// </exception>
    public static ActivationNumbers Between(DiceTotal first, DiceTotal last)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(last);
        if (last.IsBefore(first))
        {
            throw new ArgumentOutOfRangeException(nameof(last));
        }

        return new ActivationNumbers(first, last);
    }

    /// <summary>Determines whether a roll activates these numbers.</summary>
    /// <param name="roll">The complete dice roll.</param>
    /// <returns>
    /// <see langword="true"/> when the roll activates these numbers; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Includes(DiceRoll roll) => roll.HasTotalBetween(_first, _last);
}
