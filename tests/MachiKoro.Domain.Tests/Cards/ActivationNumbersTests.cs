using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class ActivationNumbersTests
{
    [Fact]
    public void At_MissingNumber_Throws()
    {
        DiceTotal missing = default!;

        Should.Throw<ArgumentNullException>(() => ActivationNumbers.At(missing));
    }

    [Fact]
    public void Between_FirstNumber_IncludesRoll()
    {
        ActivationNumbers numbers = ActivationNumbers.Between(DiceTotal.Of(9), DiceTotal.Of(10));

        numbers.Includes(DiceRoll.Of(DieResult.Of(6), DieResult.Of(3))).ShouldBeTrue();
    }

    [Fact]
    public void Between_LastNumber_IncludesRoll()
    {
        ActivationNumbers numbers = ActivationNumbers.Between(DiceTotal.Of(9), DiceTotal.Of(10));

        numbers.Includes(DiceRoll.Of(DieResult.Of(6), DieResult.Of(4))).ShouldBeTrue();
    }

    [Fact]
    public void Between_NumberOutsideRange_DoesNotIncludeRoll()
    {
        ActivationNumbers numbers = ActivationNumbers.Between(DiceTotal.Of(9), DiceTotal.Of(10));

        numbers.Includes(DiceRoll.Of(DieResult.Of(6), DieResult.Of(2))).ShouldBeFalse();
    }

    [Fact]
    public void Between_DescendingRange_Throws()
    {
        Should.Throw<ArgumentOutOfRangeException>(
            () => ActivationNumbers.Between(DiceTotal.Of(10), DiceTotal.Of(9)));
    }

    [Fact]
    public void Between_MissingFirstNumber_Throws()
    {
        DiceTotal missing = default!;

        Should.Throw<ArgumentNullException>(() => ActivationNumbers.Between(missing, DiceTotal.Of(10)));
    }

    [Fact]
    public void Between_MissingLastNumber_Throws()
    {
        DiceTotal missing = default!;

        Should.Throw<ArgumentNullException>(() => ActivationNumbers.Between(DiceTotal.Of(9), missing));
    }

    [Fact]
    public void At_DifferentRollsWithSameTotal_IncludesBoth()
    {
        ActivationNumbers numbers = ActivationNumbers.At(DiceTotal.Of(6));

        numbers.Includes(DiceRoll.Of(DieResult.Of(5), DieResult.Of(1))).ShouldBeTrue();
        numbers.Includes(DiceRoll.Of(DieResult.Of(3), DieResult.Of(3))).ShouldBeTrue();
    }
}
