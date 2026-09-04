using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed class IncomePerIndustry : IIncomeEffect
{
    private readonly Industry _industry;
    private readonly Coins _incomePerEstablishment;

    public IncomePerIndustry(Industry industry, Coins incomePerEstablishment)
    {
        _industry = industry;
        _incomePerEstablishment = incomePerEstablishment;
    }

    public IncomeClaim CreateClaim(Establishments establishments) =>
        new BankIncome(establishments.IncomeFor(_industry, _incomePerEstablishment));
}
