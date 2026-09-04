using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed class FixedIncomeFromBank : IIncomeEffect
{
    public static readonly FixedIncomeFromBank One = new(Coins.Of(1));

    private readonly Coins _amount;

    public FixedIncomeFromBank(Coins amount)
    {
        _amount = amount;
    }

    public IncomeClaim CreateClaim(Establishments establishments) => new BankIncome(_amount);
}
