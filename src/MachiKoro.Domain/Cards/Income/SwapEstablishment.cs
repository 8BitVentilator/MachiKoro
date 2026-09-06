namespace MachiKoro.Domain.Cards.Income;

/// <summary>Creates a claim to exchange establishments with another player.</summary>
public sealed class SwapEstablishment : IIncomeEffect
{
    /// <inheritdoc/>
    public IncomeClaim CreateClaim(Establishments establishments) => new EstablishmentSwap();
}
