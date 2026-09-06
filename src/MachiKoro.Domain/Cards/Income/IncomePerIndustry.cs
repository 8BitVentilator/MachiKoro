using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Creates a bank-income claim based on the number of establishments in an industry.</summary>
public sealed class IncomePerIndustry : IIncomeEffect
{
    private readonly Industry _industry;
    private readonly Coins _incomePerEstablishment;

    /// <summary>Initializes an industry-based income effect.</summary>
    /// <param name="industry">The industry whose establishments are counted.</param>
    /// <param name="incomePerEstablishment">The income claimed for each matching establishment.</param>
    public IncomePerIndustry(Industry industry, Coins incomePerEstablishment)
    {
        _industry = industry;
        _incomePerEstablishment = incomePerEstablishment;
    }

    /// <inheritdoc/>
    public IncomeClaim CreateClaim(Establishments establishments) =>
        new BankIncome(establishments.IncomeFor(_industry, _incomePerEstablishment));
}
