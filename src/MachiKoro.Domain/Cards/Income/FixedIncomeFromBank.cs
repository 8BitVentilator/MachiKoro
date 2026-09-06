using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Creates a claim for a fixed amount of income from the bank.</summary>
public sealed class FixedIncomeFromBank : IIncomeEffect
{
    /// <summary>Represents a reusable effect that claims one coin from the bank.</summary>
    public static readonly FixedIncomeFromBank One = new(Coins.Of(1));

    private readonly Coins _amount;

    /// <summary>Initializes an effect with the amount claimed from the bank.</summary>
    /// <param name="amount">The fixed amount claimed from the bank.</param>
    public FixedIncomeFromBank(Coins amount)
    {
        _amount = amount;
    }

    /// <inheritdoc/>
    public IncomeClaim CreateClaim(Establishments establishments) => new BankIncome(_amount);
}
