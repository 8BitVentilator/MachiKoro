using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Tests.Dice;

public sealed class DiceTotalTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Of_ValueOutsidePossibleTotal_Throws(int value)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => DiceTotal.Of(value));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(12)]
    public void Of_PossibleTotal_ReturnsTotal(int value)
    {
        DiceTotal.Of(value).ShouldBe(DiceTotal.Of(value));
    }

    [Theory]
    [InlineData(8, false)]
    [InlineData(9, true)]
    [InlineData(10, true)]
    [InlineData(11, false)]
    public void IsBetween_TotalComparedWithRange_ReturnsExpectedResult(int value, bool expected)
    {
        DiceTotal total = DiceTotal.Of(value);

        total.IsBetween(DiceTotal.Of(9), DiceTotal.Of(10)).ShouldBe(expected);
    }

    [Theory]
    [InlineData(9, 10, true)]
    [InlineData(10, 10, false)]
    [InlineData(11, 10, false)]
    public void IsBefore_TotalComparedWithOther_ReturnsExpectedResult(int value, int other, bool expected)
    {
        DiceTotal total = DiceTotal.Of(value);

        total.IsBefore(DiceTotal.Of(other)).ShouldBe(expected);
    }
}
