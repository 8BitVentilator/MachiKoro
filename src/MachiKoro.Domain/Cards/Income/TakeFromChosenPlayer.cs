using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed class TakeFromChosenPlayer : IIncomeEffect
{
    private readonly Coins _amount;

    public TakeFromChosenPlayer(Coins amount)
    {
        _amount = amount;
    }

    public IncomeClaim CreateClaim(Establishments establishments) => new ChosenPlayerPayment(_amount);
}
