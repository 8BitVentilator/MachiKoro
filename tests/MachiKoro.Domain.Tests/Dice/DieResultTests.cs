using MachiKoro.Domain.Dice;

namespace MachiKoro.Domain.Tests.Dice;

public sealed class DieResultTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    public void Of_ValueOutsideDie_Throws(int value)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => DieResult.Of(value));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    public void Of_ValidResult_ReturnsResult(int value)
    {
        DieResult.Of(value).ShouldBe(DieResult.Of(value));
    }
}
