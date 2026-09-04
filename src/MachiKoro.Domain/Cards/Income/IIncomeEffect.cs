namespace MachiKoro.Domain.Cards.Income;

public interface IIncomeEffect
{
    public IncomeClaim CreateClaim(Establishments establishments);
}
