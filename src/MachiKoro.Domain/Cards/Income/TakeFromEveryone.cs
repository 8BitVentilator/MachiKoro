using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Creates a claim for payment from every applicable player.</summary>
public sealed class TakeFromEveryone : IIncomeEffect
{
    private readonly Coins _amount;

    /// <summary>Initializes an effect with the amount requested from each player.</summary>
    /// <param name="amount">The amount requested from each applicable player.</param>
    public TakeFromEveryone(Coins amount)
    {
        _amount = amount;
    }

    /// <inheritdoc/>
    public IncomeClaim CreateClaim(Establishments establishments) => new EveryPlayerPayment(_amount);
}
