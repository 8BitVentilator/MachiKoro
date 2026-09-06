using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Creates a claim for payment from the active player.</summary>
public sealed class TakeFromActivePlayer : IIncomeEffect
{
    private readonly Coins _amount;

    /// <summary>Initializes an effect with the amount requested from the active player.</summary>
    /// <param name="amount">The amount requested from the active player.</param>
    public TakeFromActivePlayer(Coins amount)
    {
        _amount = amount;
    }

    /// <inheritdoc/>
    public IncomeClaim CreateClaim(Establishments establishments) => new ActivePlayerPayment(_amount);
}
