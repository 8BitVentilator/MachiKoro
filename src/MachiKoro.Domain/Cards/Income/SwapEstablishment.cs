namespace MachiKoro.Domain.Cards.Income;

public sealed class SwapEstablishment : IIncomeEffect
{
    public IncomeClaim CreateClaim(Establishments establishments) => new EstablishmentSwap();
}
