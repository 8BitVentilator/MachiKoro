using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed class TakeFromEveryone : IIncomeEffect
{
    private readonly Coins _amount;

    public TakeFromEveryone(Coins amount)
    {
        _amount = amount;
    }

    public IncomeClaim CreateClaim(Establishments establishments) => new EveryPlayerPayment(_amount);
}
