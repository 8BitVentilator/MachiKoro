using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class ActivationNumbersTests
{
    [Fact]
    public void Between_FirstNumber_IncludesRoll()
    {
        ActivationNumbers numbers = ActivationNumbers.Between(DiceRoll.Of(9), DiceRoll.Of(10));

        numbers.Includes(DiceRoll.Of(9)).ShouldBeTrue();
    }

    [Fact]
    public void Between_LastNumber_IncludesRoll()
    {
        ActivationNumbers numbers = ActivationNumbers.Between(DiceRoll.Of(9), DiceRoll.Of(10));

        numbers.Includes(DiceRoll.Of(10)).ShouldBeTrue();
    }

    [Fact]
    public void Between_NumberOutsideRange_DoesNotIncludeRoll()
    {
        ActivationNumbers numbers = ActivationNumbers.Between(DiceRoll.Of(9), DiceRoll.Of(10));

        numbers.Includes(DiceRoll.Of(8)).ShouldBeFalse();
    }

    [Fact]
    public void Between_DescendingRange_Throws()
    {
        Should.Throw<ArgumentOutOfRangeException>(
            () => ActivationNumbers.Between(DiceRoll.Of(10), DiceRoll.Of(9)));
    }
}
