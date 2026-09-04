using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed class TakeFromActivePlayer : IIncomeEffect
{
    private readonly Coins _amount;

    public TakeFromActivePlayer(Coins amount)
    {
        _amount = amount;
    }

    public IncomeClaim CreateClaim(Establishments establishments) => new ActivePlayerPayment(_amount);
}
