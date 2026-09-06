using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Tests.Dice;

public sealed class DiceRollTests
{
    [Fact]
    public void Of_OneResult_ReturnsRoll()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(5));

        roll.ShouldBe(DiceRoll.Of(DieResult.Of(5)));
    }

    [Fact]
    public void Of_OneMissingResult_Throws()
    {
        DieResult missing = default!;

        Should.Throw<ArgumentNullException>(() => DiceRoll.Of(missing));
    }

    [Fact]
    public void Of_TwoResults_ReturnsRoll()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(5), DieResult.Of(1));

        roll.ShouldBe(DiceRoll.Of(DieResult.Of(5), DieResult.Of(1)));
    }

    [Fact]
    public void Of_TwoResultsWithMissingFirst_Throws()
    {
        DieResult missing = default!;

        Should.Throw<ArgumentNullException>(() => DiceRoll.Of(missing, DieResult.Of(1)));
    }

    [Fact]
    public void Of_TwoResultsWithMissingSecond_Throws()
    {
        DieResult missing = default!;

        Should.Throw<ArgumentNullException>(() => DiceRoll.Of(DieResult.Of(1), missing));
    }

    [Fact]
    public void Of_DifferentCompositionWithSameTotal_ReturnsDifferentRoll()
    {
        DiceRoll first = DiceRoll.Of(DieResult.Of(5), DieResult.Of(1));
        DiceRoll second = DiceRoll.Of(DieResult.Of(3), DieResult.Of(3));

        first.ShouldNotBe(second);
    }

    [Fact]
    public void HasTotal_OneResult_ReturnsTrueForResult()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(5));

        roll.HasTotal(DiceTotal.Of(5)).ShouldBeTrue();
    }

    [Fact]
    public void HasTotal_TwoResults_ReturnsTrueForSum()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(5), DieResult.Of(1));

        roll.HasTotal(DiceTotal.Of(6)).ShouldBeTrue();
    }

    [Fact]
    public void HasTotalBetween_TotalInRange_ReturnsTrue()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(5), DieResult.Of(1));

        roll.HasTotalBetween(DiceTotal.Of(5), DiceTotal.Of(7)).ShouldBeTrue();
    }

    [Fact]
    public void IsDoubles_TwoEqualResults_ReturnsTrue()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(3), DieResult.Of(3));

        roll.IsDoubles().ShouldBeTrue();
    }

    [Fact]
    public void IsDoubles_TwoDifferentResults_ReturnsFalse()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(5), DieResult.Of(1));

        roll.IsDoubles().ShouldBeFalse();
    }

    [Fact]
    public void IsDoubles_OneResult_ReturnsFalse()
    {
        DiceRoll roll = DiceRoll.Of(DieResult.Of(3));

        roll.IsDoubles().ShouldBeFalse();
    }
}
