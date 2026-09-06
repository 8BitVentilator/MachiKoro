namespace MachiKoro.Domain.Cards.Income;

/// <summary>Defines an income effect produced by an activated establishment.</summary>
public interface IIncomeEffect
{
    /// <summary>Creates the claim that represents this income effect.</summary>
    /// <param name="establishments">The establishments available when calculating the claim.</param>
    /// <returns>The resulting income claim.</returns>
    public IncomeClaim CreateClaim(Establishments establishments);
}
