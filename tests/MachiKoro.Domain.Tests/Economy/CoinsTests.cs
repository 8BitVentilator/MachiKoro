using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Economy;

public sealed class CoinsTests
{
    [Fact]
    public void Of_NegativeAmount_Throws()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => Coins.Of(-1));
    }

    [Fact]
    public void Of_Zero_EqualsZero()
    {
        Coins.Of(0).ShouldBe(Coins.Zero);
    }

    [Fact]
    public void Receive_AddsIncome()
    {
        Coins.Of(3).Receive(Coins.Of(2)).ShouldBe(Coins.Of(5));
    }

    [Fact]
    public void Pay_SufficientCoins_SubtractsCost()
    {
        Coins.Of(5).Pay(Coins.Of(2)).ShouldBe(Coins.Of(3));
    }

    [Fact]
    public void Pay_InsufficientCoins_LeavesZero()
    {
        Coins.Of(1).Pay(Coins.Of(4)).ShouldBe(Coins.Zero);
    }

    [Theory]
    [InlineData(3, 3, true)]
    [InlineData(4, 3, true)]
    [InlineData(2, 3, false)]
    public void CanAfford_ComparesAgainstCost(int owned, int cost, bool expected)
    {
        Coins.Of(owned).CanAfford(Coins.Of(cost)).ShouldBe(expected);
    }

    [Fact]
    public void ToString_ReturnsPlainAmount()
    {
        Coins.Of(12).ToString().ShouldBe("12");
    }
}
