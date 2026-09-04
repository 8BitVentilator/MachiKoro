using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Tests.Dice;

public sealed class DiceRollTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Of_ValueOutsidePossibleSum_Throws(int value)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => DiceRoll.Of(value));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(12)]
    public void Of_PossibleSum_ReturnsRoll(int value)
    {
        DiceRoll.Of(value).ShouldBe(DiceRoll.Of(value));
    }
}
